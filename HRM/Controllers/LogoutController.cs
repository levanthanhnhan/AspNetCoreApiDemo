using System;
using HRM.Models;
using HRM.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HRM.Controllers
{
    [Route("api/Logout")]
    [EnableCors("AllowMyOrigin")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly IConfiguration _config;

        public LogoutController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Logout([FromBody] string userName)
        {
            var loginService = new LoginService(_config);
            loginService.RevokeRefreshToken(userName);
            return Ok();
        }
    }
}
