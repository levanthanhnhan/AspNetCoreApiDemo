using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class DepartmentObj
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public int LeaderStaffId { get; set; }
        public string LeaderName { get; set; }
    }
}
