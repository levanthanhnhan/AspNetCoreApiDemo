using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Models
{
    public class StaffList : Staff
    {
        public string SexName { get; set; }
        public string DepartmentName { get; set; }
        public string PositionName { get; set; }
        public bool isSelected { get; set; }
    }
}
