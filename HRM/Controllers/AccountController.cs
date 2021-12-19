using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HRM.Models;
using HRM.Services;
using Microsoft.Extensions.Configuration;
using System.Web;
using HRM.Common;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRM.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AccountController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            StaffService staffService = new StaffService(_config);
            RoleService roleService = new RoleService(_config);
            List<Staff> listStaff;
            List<Role> listRole;

            try
            {
                listStaff = staffService.GetStaffs();
                listRole = roleService.GetRoles();
                return Ok(new { _listStaff = listStaff, _listRole = listRole });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Create")]
        public ActionResult Create([FromBody] Account account)
        {
            AccountService accoutService = new AccountService(_config);
            try
            {
                if (accoutService.FindAccountByUserName(account.UserName) == null)
                {
                    accoutService.CreateAccount(account);
                    return Ok(new { _statusCode = 1, _message = "Create Successfully!", _username = account.UserName, _password = account.Password, _email = account.Email });
                }
                else
                {
                    return Ok( new {_statusCode = 0, _message = "Account is available." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
