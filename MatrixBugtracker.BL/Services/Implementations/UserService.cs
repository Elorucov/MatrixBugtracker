using AutoMapper;
using MatrixBugtracker.Abstractions;
using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, ITokenService tokenService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepo = unitOfWork.GetRepository<IUserRepository>();
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
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

            newUser.IsEmailConfirmed = true; // TODO: remove this after implementing e-mail confirmation

            await _userRepo.AddAsync(newUser);
            await _unitOfWork.CommitAsync();

            return new ResponseDTO<bool>(true);
        }
    }
}
