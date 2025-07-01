using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Admin;
using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using MatrixBugtracker.Domain.Entities;
using MatrixBugtracker.Domain.Enums;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class UserService : IUserService
    {
        private static readonly ConcurrentDictionary<int, User> _cachedUsers = new ConcurrentDictionary<int, User>();

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly IFileRepository _fileRepo;
        private readonly IRefreshTokenRepository _refreshTokenRepo;

        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly INotificationService _notificationService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher,
            ITokenService tokenService, INotificationService notificationService, IUserIdProvider userIdProvider,
            IConfiguration config, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepo = unitOfWork.GetRepository<IUserRepository>();
            _fileRepo = unitOfWork.GetRepository<IFileRepository>();
            _refreshTokenRepo = unitOfWork.GetRepository<IRefreshTokenRepository>();

            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _notificationService = notificationService;
            _userIdProvider = userIdProvider;
            _config = config;
            _mapper = mapper;
        }

        #region Private methods

        public async Task<User> GetSingleUserAsync(int userId)
        {
            if (!_cachedUsers.TryGetValue(userId, out User user))
            {
                user = await _userRepo.GetByIdWithIncludeAsync(userId);
                if (user != null) _cachedUsers.TryAdd(userId, user);
            }

            return user;
        }

        #endregion

        public async Task<ResponseDTO<TokenDTO>> LoginAsync(LoginRequestDTO request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null) return ResponseDTO<TokenDTO>.Unauthorized(Errors.WrongEmailOrPassword);

            bool isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.Password);
            if (!isPasswordValid) return ResponseDTO<TokenDTO>.Unauthorized(Errors.WrongEmailOrPassword);

            // Create refresh token
            string rToken = _tokenService.GenerateRefreshToken();
            var rTokenExpiresAt = DateTime.Now.AddDays(Convert.ToInt32(_config["Jwt:RefreshTokenExpirationDays"]));

            RefreshToken rte = new RefreshToken
            {
                User = user,
                Token = rToken,
                ExpirationTime = rTokenExpiresAt
            };

            await _refreshTokenRepo.AddAsync(rte);
            await _unitOfWork.CommitAsync();

            var tokenDTO = _tokenService.GetToken(user.Id);
            tokenDTO.RefreshToken = rte.Token;
            tokenDTO.RefreshTokenExpiresAt = rte.ExpirationTime;

            return new ResponseDTO<TokenDTO>(tokenDTO);
        }

        public async Task<ResponseDTO<TokenDTO>> RefreshAsync(int userId, string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken)) return ResponseDTO<TokenDTO>.BadRequest();

            RefreshToken rte = await _refreshTokenRepo.GetByTokenAsync(refreshToken, userId);
            if (rte == null) return ResponseDTO<TokenDTO>.BadRequest();

            if (rte.ExpirationTime <= DateTime.Now)
            {
                _refreshTokenRepo.Delete(rte);
                await _unitOfWork.CommitAsync();
                return ResponseDTO<TokenDTO>.Unauthorized(Errors.RefreshTokenExpired);

            }

            if (rte.User == null) return ResponseDTO<TokenDTO>.BadRequest();

            var tokenDTO = _tokenService.GetToken(rte.UserId);
            rte.Token = _tokenService.GenerateRefreshToken();
            rte.ExpirationTime = DateTime.Now.AddDays(Convert.ToInt32(_config["Jwt:RefreshTokenExpirationDays"]));

            _refreshTokenRepo.Update(rte);
            await _unitOfWork.CommitAsync();

            tokenDTO.RefreshToken = rte.Token;
            tokenDTO.RefreshTokenExpiresAt = rte.ExpirationTime;

            return new ResponseDTO<TokenDTO>(tokenDTO);
        }

        public async Task<ResponseDTO<bool>> CreateUserAsync(RegisterRequestDTO request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user != null) return ResponseDTO<bool>.BadRequest(Errors.EmailInUse);

            User newUser = null;
            newUser = _mapper.Map(request, newUser);
            newUser.Password = _passwordHasher.HashPassword(request.Password);

            newUser.Role = UserRole.Tester;
            newUser.IsEmailConfirmed = true; // TODO: remove this after implementing e-mail confirmation

            await _userRepo.AddAsync(newUser);
            await _unitOfWork.CommitAsync();

            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<UserDTO>> GetByIdAsync(int userId)
        {
            var user = await GetSingleUserAsync(userId);
            if (user == null) return ResponseDTO<UserDTO>.NotFound(Errors.NotFoundUser);

            UserDTO dto = null;
            dto = _mapper.Map(user, dto);

            return new ResponseDTO<UserDTO>(dto);
        }

        public async Task<ResponseDTO<bool>> SetUserRoleAsync(SetRoleRequestDTO request)
        {
            int currentUserId = _userIdProvider.UserId;
            if (request.UserId == currentUserId) return ResponseDTO<bool>.BadRequest(Errors.CannotRevokeAdminRoleFromCurrentUser);

            var user = await GetSingleUserAsync(request.UserId);
            if (user == null) return ResponseDTO<bool>.NotFound();
            if (user.Role == request.Role) return ResponseDTO<bool>.BadRequest(Errors.RoleIsSame);

            // Set default moderator name if user's role changed from tester to higher role.
            // We get the count of users with moder name, add 1 to the count and assign it to the moder name.
            if (user.Role == UserRole.Tester && user.ModeratorName == null)
            {
                int modersTotalCount = await _userRepo.GetUsersCountWithModeratorNamesAsync();
                user.ModeratorName = $"{Common.Moderator} #{modersTotalCount + 1}";
            }
            user.Role = request.Role;

            _userRepo.Update(user);
            await _notificationService.SendToUserAsync(user.Id, true, UserNotificationKind.RoleChanged, LinkedEntityType.Role, (int)request.Role);

            await _unitOfWork.CommitAsync();

            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> EditAsync(UserEditDTO request)
        {
            int userId = _userIdProvider.UserId;

            User user = await GetSingleUserAsync(userId);
            _mapper.Map(request, user);

            _userRepo.Update(user);
            await _unitOfWork.CommitAsync();

            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> ChangePhotoAsync(int photoFileId)
        {
            int userId = _userIdProvider.UserId;

            var file = await _fileRepo.GetByIdAsync(photoFileId);
            if (file == null) return ResponseDTO<bool>.NotFound();

            if (file.CreatorId != userId) return ResponseDTO<bool>.Forbidden();
            if (!file.IsImage()) return ResponseDTO<bool>.BadRequest(Errors.FileIsNotImage);

            User user = await GetSingleUserAsync(userId);
            user.PhotoFile = file;

            _userRepo.Update(user);
            await _unitOfWork.CommitAsync();

            return new ResponseDTO<bool>(true);
        }
    }
}
