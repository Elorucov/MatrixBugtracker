using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.DTOs.Users;
using MatrixBugtracker.BL.Extensions;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Enums;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using System.Collections.Concurrent;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class UserService : IUserService
    {
        private static readonly ConcurrentDictionary<int, User> _cachedUsers = new ConcurrentDictionary<int, User>();

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly IFileRepository _fileRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher,
            ITokenService tokenService, IUserIdProvider userIdProvider, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepo = unitOfWork.GetRepository<IUserRepository>();
            _fileRepo = unitOfWork.GetRepository<IFileRepository>();
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _userIdProvider = userIdProvider;
            _mapper = mapper;
        }

        public async Task<ResponseDTO<TokenDTO>> LoginAsync(LoginRequestDTO request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null) return ResponseDTO<TokenDTO>.Unauthorized(Errors.WrongEmailOrPassword);

            bool isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.Password);
            if (!isPasswordValid) return ResponseDTO<TokenDTO>.Unauthorized(Errors.WrongEmailOrPassword);

            return new ResponseDTO<TokenDTO>(_tokenService.GetToken(user.Id));
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

        private async Task<User> GetSingleUserAsync(int userId)
        {
            if (!_cachedUsers.TryGetValue(userId, out User user))
            {
                user = await _userRepo.GetByIdWithIncludeAsync(userId);
                if (user != null) _cachedUsers.TryAdd(userId, user);
            }

            return user;
        }

        public async Task<ResponseDTO<UserDTO>> GetByIdAsync(int userId)
        {
            var user = await GetSingleUserAsync(userId);
            if (user == null) return ResponseDTO<UserDTO>.NotFound();

            UserDTO dto = null;
            dto = _mapper.Map(user, dto);

            return new ResponseDTO<UserDTO>(dto);
        }

        public async Task<UserRole?> GetUserRoleAsync(int userId)
        {
            var user = await GetSingleUserAsync(userId);
            if (user == null) return null;

            return user.Role;
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
