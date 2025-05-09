using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public UserService(IConfiguration config, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _userRepo = unitOfWork.GetRepository<IUserRepository>();
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<ResponseDTO<TokenDTO>> LoginAsync(LoginRequestDTO request)
        {
            var user = await _userRepo.GetByEmailAsync(request.Email);
            if (user == null) return ResponseDTO<TokenDTO>.Error(StatusCodes.Status401Unauthorized, Errors.WrongEmailOrPassword);

            bool isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.Password);
            if (!isPasswordValid) return ResponseDTO<TokenDTO>.Error(StatusCodes.Status401Unauthorized, Errors.WrongEmailOrPassword);

            return new ResponseDTO<TokenDTO>(_tokenService.GetToken(user.Id));
        }
    }
}
