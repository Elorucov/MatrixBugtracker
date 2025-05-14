using MatrixBugtracker.BL.DTOs.Auth;
using MatrixBugtracker.BL.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MatrixBugtracker.BL.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public TokenDTO GetToken(int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
            SigningCredentials creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, _config["Jwt:Issuer"]),
            };

            DateTime expirationTime = DateTime.Now.AddMinutes(Convert.ToInt32(_config["Jwt:ExpirationMinutes"]));

            JwtSecurityToken jwt = new JwtSecurityToken(
                claims: claims,
                expires: expirationTime,
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new TokenDTO
            {
                UserId = userId,
                AccessToken = token,
                AccessTokenExpiresAt = expirationTime
            };
        }

        public string GenerateRefreshToken()
        {
            var random = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(random);
            return Convert.ToBase64String(random);
        }
    }
}
