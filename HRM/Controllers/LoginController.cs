using System;
using HRM.Models;
using HRM.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using HRM.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRM.Controllers
{
    [Route("api/Login")]
    [EnableCors("AllowMyOrigin")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Login([FromBody] Account account)
        {
            var loginService = new LoginService(_config);
            account = loginService.Login(account.UserName, account.Password);

            if (account != null)
            {
                var authService = new AuthService(_config);

                // Add claims token
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, account.UserName),
                    new Claim(ClaimTypes.NameIdentifier, account.StaffId.ToString())
                };

                // Generate tokens
                DateTime accessTokenExpires;
                var accessToken = authService.GenerateAccessToken(claims, out accessTokenExpires);
                var refreshToken = authService.GenerateRefreshToken();

                // Save refresh token to DB
                var refreshTokenExpires = DateTime.Now.AddMinutes(Constant.TOKEN_REFRESH_EXPIRED_MINUTES);
                loginService.UpdateRefreshToken(refreshToken, refreshTokenExpires, account);

                // Result json object
                var objResult = new
                {
                    token = new
                    {
                        accessToken = accessToken,
                        accessTokenExpires = accessTokenExpires,
                        refreshToken = refreshToken,
                        refreshTokenExpires = refreshTokenExpires
                    },
                    account = new
                    {
                        userName = account.UserName,
                        userId = account.StaffId,
                        roleId = account.RoleId,
                    }
                };

                return Ok(objResult);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
