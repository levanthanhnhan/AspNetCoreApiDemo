using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HRM.Models;
using Microsoft.Extensions.Configuration;
using HRM.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HRM.Controllers
{
    [Route("api/Position")]
    [EnableCors("AllowMyOrigin")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly ILogger<PositionController> _logger;
        private readonly IConfiguration _config;

        public PositionController(ILogger<PositionController> logger, IConfiguration config)
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
        public List<Position> GetPositions()
        {
            var position = new PositionService(_config);
            return position.GetPositions();
        }
    }
}
