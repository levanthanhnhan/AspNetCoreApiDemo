using HRM.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using HRM.Models;

namespace HRM.Services
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        private readonly string _secretKey;

        public AuthService(IConfiguration config)
        {
            _config = config;

            var appSettingsSection = _config.GetSection("JWTToken");
            var appSettings = appSettingsSection.Get<JWTToken>();
            _secretKey = appSettings.SecretKey;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="accessTokenExpires"></param>
        /// <returns></returns>
        public string GenerateAccessToken(IEnumerable<Claim> claims, out DateTime accessTokenExpires)
        {
            accessTokenExpires = DateTime.Now.AddMinutes(Constant.TOKEN_EXPIRED_MINUTES);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            var jwtToken = new JwtSecurityToken(
                issuer: "https://localhost:5001/",
                audience: "https://localhost:5001/",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: accessTokenExpires,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            SecurityToken securityToken;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                ValidateLifetime = false
            };

            var principal = new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
