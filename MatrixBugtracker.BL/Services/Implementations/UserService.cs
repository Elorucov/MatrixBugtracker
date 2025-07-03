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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class UserService : IUserService
    {
        private static readonly ConcurrentDictionary<int, User> _cachedUsers = new ConcurrentDictionary<int, User>();

        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly IConfirmationRepository _confirmsRepo;
        private readonly IFileRepository _fileRepo;
        private readonly IRefreshTokenRepository _refreshTokenRepo;

        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IGenerator _generator;
        private readonly IEmailService _emailService;
        private readonly INotificationService _notificationService;
        private readonly IUserIdProvider _userIdProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IGenerator generator, IEmailService emailService,
            ITokenService tokenService, INotificationService notificationService, IUserIdProvider userIdProvider, IHttpContextAccessor httpContextAccessor,
            IConfiguration config, IMapper mapper, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _userRepo = unitOfWork.GetRepository<IUserRepository>();
            _confirmsRepo = unitOfWork.GetRepository<IConfirmationRepository>();
            _fileRepo = unitOfWork.GetRepository<IFileRepository>();
            _refreshTokenRepo = unitOfWork.GetRepository<IRefreshTokenRepository>();

            _passwordHasher = passwordHasher;
            _generator = generator;
            _emailService = emailService;
            _tokenService = tokenService;
            _notificationService = notificationService;
            _userIdProvider = userIdProvider;
            _httpContextAccessor = httpContextAccessor;
            _config = config;
            _mapper = mapper;
            _logger = logger;
        }

        #region Non-controller methods

        public async Task<User> GetSingleUserAsync(int userId)
        {
            if (!_cachedUsers.TryGetValue(userId, out User user))
            {
                user = await _userRepo.GetByIdWithPhotoAsync(userId);
                if (user != null) _cachedUsers.TryAdd(userId, user);
            }

            return user;
        }

        public async Task<List<KeyValuePair<string, string>>> GetNamesAndEmailsAsync(IEnumerable<int> userIds)
        {
            return await _userRepo.GetEmailsAsync(userIds);
        }

        #endregion

        public async Task<ResponseDTO<TokenDTO>> LoginAsync(LoginRequestDTO request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null) return ResponseDTO<TokenDTO>.Unauthorized(Errors.WrongEmailOrPassword);

            bool isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.Password);
            if (!isPasswordValid) return ResponseDTO<TokenDTO>.Unauthorized(Errors.WrongEmailOrPassword);

            if (!user.IsEmailConfirmed)
            {
                if (string.IsNullOrEmpty(request.ConfirmationCode))
                    return ResponseDTO<TokenDTO>.Unauthorized(Errors.AccountNotConfirmed);

                var confirmation = await _confirmsRepo.GetByUserIdAsync(user.Id, EmailConfirmationKind.Registration);
                if (confirmation == null)
                {
                    _logger.LogError("User {0}'s email ({1}) is not confirmed, but confirmation code is not found in DB!", user.Id, user.Email);
                    return ResponseDTO<TokenDTO>.InternalServerError();
                }

                if (confirmation.Code != request.ConfirmationCode)
                    return ResponseDTO<TokenDTO>.BadRequest(Errors.InvalidConfirmationCode);

                user.IsEmailConfirmed = true;
                _userRepo.Update(user);

                _confirmsRepo.Delete(confirmation);
            }

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
            await _userRepo.AddAsync(newUser);

            string code = _generator.GenerateDigitsCode();
            await _confirmsRepo.AddAsync(new Confirmation
            {
                User = newUser,
                Kind = EmailConfirmationKind.Registration,
                Code = code
            });

            await _unitOfWork.CommitAsync();
            await _emailService.SendMailAsync(newUser.Email, Common.Email_UserRegistrationSubject, string.Format(Common.Email_UserRegistrationText, newUser.FirstName, code));

            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> SendPasswordResetConfirmationAsync(string email)
        {
            if (string.IsNullOrEmpty(email)) return ResponseDTO<bool>.BadRequest();

            var user = await _userRepo.GetByEmailAsync(email);
            if (user == null) return ResponseDTO<bool>.NotFound();

            string code = _generator.GenerateDigitsCode();

            // If DB has a confirmation, we change code and send mail again with new code.
            var confirmation = await _confirmsRepo.GetByUserIdAsync(user.Id, EmailConfirmationKind.PasswordReset);
            if (confirmation == null)
            {
                confirmation = new Confirmation
                {
                    User = user,
                    Kind = EmailConfirmationKind.PasswordReset,
                    Code = code
                };
                await _confirmsRepo.AddAsync(confirmation);
            }
            else
            {
                confirmation.Code = code;
                _confirmsRepo.Update(confirmation);
            }

            await _unitOfWork.CommitAsync();
            await _emailService.SendMailAsync(user.Email, Common.Email_PasswordResetSubject, string.Format(Common.Email_PasswordResetText, user.FirstName, code));

            return new ResponseDTO<bool>(true);
        }

        public async Task<ResponseDTO<bool>> ResetPasswordAsync(PasswordResetRequestDTO request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null) return ResponseDTO<bool>.NotFound();

            var confirmation = await _confirmsRepo.GetByUserIdAsync(user.Id, EmailConfirmationKind.PasswordReset);
            if (confirmation == null) return ResponseDTO<bool>.BadRequest();
            if (confirmation.Code != request.Code) return ResponseDTO<bool>.BadRequest(Errors.InvalidConfirmationCode);

            user.Password = _passwordHasher.HashPassword(request.Password);
            _userRepo.Update(user);

            _confirmsRepo.Delete(confirmation);
            await _unitOfWork.CommitAsync();

            // DI via class constructor leads to a crash on startup!
            var notificationService = _httpContextAccessor.HttpContext.RequestServices.GetService<INotificationService>();
            await notificationService.SendToUserAsync(user.Id, true, UserNotificationKind.PasswordReset, Common.PasswordResetNotification);

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

            var user = await _userRepo.GetByIdWithProductsAsync(request.UserId);
            if (user == null) return ResponseDTO<bool>.NotFound();
            if (user.Role == request.Role) return ResponseDTO<bool>.BadRequest(Errors.RoleIsSame);

            // Don't downgrade role from employee if he has a products.
            if ((byte)user.Role <= 2 && (int)request.Role > 2 && user.CreatedProducts.Count > 0)
            {
                _logger.LogWarning("User {0} as {1} has {2} products. Cannot downgrade role to {3}", user.Id, user.Role, user.CreatedProducts.Count, request.Role);
                return ResponseDTO<bool>.BadRequest(Errors.CannotChangeRole);
            }

            // Set default moderator name if user's role changed from tester to higher role.
            // We get the count of users with moder name, add 1 to the count and assign it to the moder name.
            if (user.Role == UserRole.Tester && user.ModeratorName == null)
            {
                int modersTotalCount = await _userRepo.GetUsersCountWithModeratorNamesAsync();
                user.ModeratorName = $"{Common.Moderator} #{modersTotalCount + 1}";
            }
            user.Role = request.Role;

            _userRepo.Update(user);

            string notificationText = string.Format(Common.RoleChangedNotification, request.Role.ToString());
            await _notificationService.SendToUserAsync(user.Id, true, UserNotificationKind.RoleChanged, notificationText, LinkedEntityType.Role, (int)request.Role);

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
