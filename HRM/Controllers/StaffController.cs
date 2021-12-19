using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HRM.Models;
using HRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HRM.Controllers
{
    [Route("api/Staff")]
    [EnableCors("Allow MyOrigin")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly ILogger<StaffController> _logger;
        private readonly IConfiguration _config;

        public StaffController(ILogger<StaffController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        /// <summary>
        /// Get all staffs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public List<Staff> GetStaffs()
        {
            var user = new StaffService(_config);
            return user.GetStaffs();
        }

        /// <summary>
        /// Get Staff By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "Get")]
        [Authorize]
        public Staff GetStaffById(int id)
        {
            var service = new StaffService(_config);
            return service.GetStaffById(id);
        }

        /// <summary>
        /// Update Staff
        /// </summary>
        /// <param name="staff"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Update")]
        public IActionResult UpdateStaff([FromBody] Staff staff)
        {
            var service = new StaffService(_config);
            bool result = service.UpdateStaff(staff);

            return result ? Ok(new { status = "SUCCESS" }) : Ok(new { status = "FAILED" });
        }

        /// <summary>
        /// Register Staff
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        [Authorize]
        public IActionResult Register([FromBody] Staff info)
        {
            StaffService staffRegisterService = new StaffService(_config);
            bool result = staffRegisterService.RegisterStaff(info);

            return result ? Ok(new { status = "SUCCESS" }) : Ok(new { status = "FAILED" });
        }

        /// <summary>
        /// Delete Staffs
        /// </summary>
        /// <param name="checkedList"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("DeleteStaffs")]
        public int DeleteStaffs(ArrayList checkedList)
        {
            var user = new StaffService(_config);
            return user.DeleteStaffs(checkedList);
        }

        /// <summary>
        /// Get Relation Staffs
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("GetRelationStaffs")]
        public IActionResult GetRelationStaffs(StaffSearch Model)
        {
            var user = new StaffService(_config);

            var objResult = new
            {
                list = user.GetRelationStaffs(Model.DepartmentId, Model.PositionId, Model.StaffName),
                pageCount = Common.Constant.ITEM_PER_PAGE
            };

            return Ok(objResult);
        }

        /// <summary>
        /// Get Staff Infomation By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("StaffInfoById/{id}")]
        public StaffList GetStaffInfoById(int id)
        {
            var service = new StaffService(_config);
            return service.GetStaffInfoById(id);
        }

        /// <summary>
        /// Get Staff By Year Using Chart
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetStaffByYear")]
        public object GetStaffByYear()
        {
            var user = new StaffService(_config);
            var results = user.GetStaffs().GroupBy(staff => staff.ContractDate).Select(g => new
            {
                Year = g.Key,
                Count = g.Count()
            });

            return results.OrderBy(x => x.Year).ToList();
        }

        /// <summary>
        /// Get Staff Department Using Chart
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("StaffDepartment/{id}")]
        public List<StaffDepartmentList> GetStaffDepartment(int id)
        {
            var user = new StaffService(_config);
            return user.GetStaffDepartment(id);
        }
    }

    public class StaffSearch
    {
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
        public string StaffName { get; set; }
    }
}
