using HRM.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using HRM.Models;
using System.Collections.Generic;

[Route("api/Dashboard")]
[EnableCors("AllowMyOrigin")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IConfiguration _config;

    public DashboardController(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetStaffByYear")]
    [Authorize]
    public object GetStaffByYear()
    {
        var user = new StaffService(_config);
        var results = user.GetStaffs().GroupBy(staff => staff.ContractDate.ToString("yyyy-MM")).Select(g => new
        {
            Year = g.Key,
            Count = g.Count()
        });

        return results.OrderBy(x => x.Year).ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("GetDepartments")]
    [Authorize]
    public List<DepartmentChart> GetDepartments()
    {
        var user = new DepartmentChartService(_config);
        return user.GetDepartments();
    }
}
