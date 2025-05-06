using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.DTOs.Infra;
using MatrixBugtracker.BL.Resources;
using MatrixBugtracker.BL.Services.Abstractions;
using MatrixBugtracker.DAL.Entities;
using MatrixBugtracker.DAL.Enums;
using MatrixBugtracker.DAL.Repositories.Abstractions;
using MatrixBugtracker.DAL.Repositories.Abstractions.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public UserService(IConfiguration config, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _userRepo = unitOfWork.GetRepository<IUserRepository>();
            _roleRepo = unitOfWork.GetRepository<IRoleRepository>();
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<ResponseDTO<bool>> CreateFirstUserAsync()
        {
            string email = _config["FirstUser:Email"];
            var existUser = await _userRepo.GetByEmailAsync(email);
            if (existUser != null) return ResponseDTO<bool>.Error(StatusCodes.Status400BadRequest, Errors.FirstUserCreated);

            User newUser = new User()
            {
                FirstName = _config["FirstUser:FirstName"],
                LastName = _config["FirstUser:LastName"],
                Email = email,
                Password = _passwordHasher.HashPassword(_config["FirstUser:Password"])
            };

            var adminRole = await _roleRepo.GetByEnumAsync(RoleEnum.Admin);

            newUser.UserRoles = new List<UserRole>()
            {
                new UserRole() {
                    User = newUser,
                    Role = adminRole
                }
            };

            await _userRepo.AddAsync(newUser);
            await _unitOfWork.CommitAsync();
            return new ResponseDTO<bool>(true);
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
