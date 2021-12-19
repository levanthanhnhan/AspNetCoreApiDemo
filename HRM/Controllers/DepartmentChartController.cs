using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRM.Models;
using HRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HRM.Controllers
{
    [Route("api/DepartmentChart")]
    [EnableCors("AllowMyOrigin")]
    [ApiController]
    public class DepartmentChartController : ControllerBase
    {
        private readonly IConfiguration _config;

        public DepartmentChartController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public List<DepartmentChart> GetDepartments()
        {
            var user = new DepartmentChartService(_config);
            return user.GetDepartments();
        }
    }
}