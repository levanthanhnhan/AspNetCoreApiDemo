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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRM.Controllers
{
    [Route("api/Department")]
    [EnableCors("AllowMyOrigin")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly IConfiguration _config;

        public DepartmentController(ILogger<DepartmentController> logger, IConfiguration config)
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
        public List<Department> GetDepartments()
        {
            var service = new DepartmentService(_config);
            return service.GetAllDepartment();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public Department GetDepartmentByID(int id)
        {
            DepartmentService service = new DepartmentService(_config);
            return service.GetDepartmentByID(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depart"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Create")]
        public ActionResult Create([FromBody] Department depart)
        {
            DepartmentService service = new DepartmentService(_config);
            try
            {
                if (service.FindDepartmentByName(depart.Name) == null)
                {
                    int result = service.RegisterDepartment(depart);
                    return Ok(new { _statusCode = 1, _message = "Create Successfully !"});
                }
                else
                {
                    return Ok(new { _statusCode = 0, _message = "Department has exist." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="depart"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Update")]
        public ActionResult Update([FromBody] Department depart)
        {
            DepartmentService service = new DepartmentService(_config);
            try
            {
                int result = service.UpdateDepartment(depart);
                return Ok(new { _statusCode = 1, _message = "Update Successfully !" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Delete")]
        public ActionResult DeleteDepartment([FromBody] int departmentID)
        {
            DepartmentService service = new DepartmentService(_config);
            if (service.checkDepartmentHasStaff(departmentID))
            {
                return Ok(new { _statusCode = 2, _message = "Department has exist staff !" });
            }
            try
            {
                int result = service.DeleteDepartment(departmentID);
                return Ok(new { _statusCode = 1, _message = "Delete Successfully !" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Search")]
        public List<Department> SearchDepartment([FromBody] DepartmentSearch search)
        {
            DepartmentService service = new DepartmentService(_config);
            return service.SearchDepartments(search.DepartmentName);
        }

        public class DepartmentSearch
        {
            public string DepartmentName { get; set; }
        }
    }
}
