using HRM.Common;
using HRM.Models;
using HRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Controllers
{
    [Route("api/ChangePassword")]
    [EnableCors("AllowMyOrigin")]
    [ApiController]
    public class ChangePasswordController: ControllerBase
    {
        private readonly IConfiguration _config;

        public ChangePasswordController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult ChangePassword([FromBody] Account account)
        {
            ChangePasswordService staffChangePasswordService = new ChangePasswordService(_config);
            string currentPassword = staffChangePasswordService.CheckPassword(account);
            string msg = string.Empty;

            if (currentPassword == DBUtils.EncryptPassword(account.OldPassword))
            {
                int result = staffChangePasswordService.ChangePassword(account);

                if (result == 0)
                    msg = "Change password is fail!";
                else
                    msg = "Change password is success!";
            }
            else
                msg = "Old password is incorrect!";

            return Ok(new { _msg = msg });
        }
    }
}
