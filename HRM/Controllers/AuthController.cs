using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRM.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using HRM.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using HRM.Common;

namespace HRM.Controllers
{
    [Route("api/Auth")]
    [EnableCors("AllowMyOrigin")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Refresh")]
        public IActionResult Refresh(JWTToken token)
        {
            var authService = new AuthService(_config);
            var loginService = new LoginService(_config);
            AccountService accountService = new AccountService(_config);

            // Verified Expired Token
            var principal = authService.GetPrincipalFromExpiredToken(token.AccessToken);
            var username = principal.Identity.Name;

            var account = accountService.GetAccounts().SingleOrDefault(u => u.UserName == username);
            if (account == null || account.RefreshToken != token.RefreshToken || IsExpired(account.TokenExpired))
            {
                return Forbid();
            }

            // Generate New Token
            DateTime accessTokenExpires;
            var newAccessToken = authService.GenerateAccessToken(principal.Claims, out accessTokenExpires);
            var newRefreshToken = authService.GenerateRefreshToken();

            // Save refresh token to DB
            var refreshTokenExpires = DateTime.Now.AddMinutes(Constant.TOKEN_REFRESH_EXPIRED_MINUTES);

            loginService.UpdateRefreshToken(newRefreshToken, refreshTokenExpires, account);

            // Result json object
            var objResult = new
            {
                token = new
                {
                    accessToken = newAccessToken,
                    accessTokenExpires = accessTokenExpires,
                    refreshToken = newRefreshToken,
                    refreshTokenExpires = refreshTokenExpires
                },
                account = new
                {
                    userName = account.UserName,
                    userId = account.StaffId
                }
            };

            return Ok(objResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Revoke")]
        public IActionResult Revoke()
        {
            var username = User.Identity.Name;
            var loginService = new LoginService(_config);
            loginService.RevokeRefreshToken(username);

            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expired"></param>
        /// <returns></returns>
        private bool IsExpired(DateTime expired)
        {
            return (expired > DateTime.Now) ? false : true;
        }
    }
}
