using HRM.Models;
using HRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Controllers
{
    [Route("api/Role")]
    [EnableCors("AllowMyOrigin")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly IConfiguration _config;

        public RoleController(ILogger<RoleController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public List<Role> GetRoles()
        {
            var role = new RoleService(_config);
            return role.GetRoles();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("GetRoleAccesss")]
        public List<RoleAccess> GetRoleAccesss()
        {
            var roleAccess = new RoleService(_config);
            return roleAccess.GetRoleAccesss();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleAccessList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("SaveChanges")]
        public IActionResult SaveChanges([FromBody] RoleAccessList roleAccessList)
        {
            try
            {
                var service = new RoleAccessService(_config);

                // Update RoleAccess
                if (service.DeleteRoleAccess())
                {
                    // Delete Role
                    if (roleAccessList.ListRoleDelete.Count > 0)
                    {
                        service.DeleteRole(roleAccessList.ListRoleDelete);
                    }

                    // Insert Or Update Role
                    if (roleAccessList.ListRoleInsertUpdate.Count > 0)
                    {
                        service.InsertUpdateRole(roleAccessList.ListRoleInsertUpdate);
                    }

                    // Save RoleAccess
                    service.SaveRoleAccess(roleAccessList.ListRoleAccess);
                }

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("GetAccesssByRoleId")]
        public List<Access> GetAccesssByRoleId([FromBody] int roleId)
        {
            var roleAccess = new RoleService(_config);
            return roleAccess.GetAccesssByRoleId(roleId);
        }
    }
}
