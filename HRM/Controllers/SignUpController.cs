using System;
using HRM.Models;
using HRM.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HRM.Controllers
{
    [Route("api/SignUp")]
    [EnableCors("AllowMyOrigin")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private readonly IConfiguration _config;

        public SignUpController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SignUp([FromBody] string userName)
        {
            AccountService accoutService = new AccountService(_config);
            try
            {
                if (accoutService.FindAccountByUserName(userName) == null)
                {
                    Account account = new Account();
                    account.UserName = userName;
                    account.Password = userName;

                    accoutService.CreateAccount(account);
                    return Ok(new { _statusCode = 1, _message = "Create Successfully!" });
                }
                else
                {
                    return Ok(new { _statusCode = 0, _message = "Account is available." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
