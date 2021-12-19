using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRM.Models
{
    public class RoleAccess
    {
        public int AccessId { get; set; }
        public string AccessName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class RoleAccessList
    {
        public List<Role> ListRoleDelete { get; set; }
        public List<Role> ListRoleInsertUpdate { get; set; }
        public List<RoleAccess> ListRoleAccess { get; set; }
    }
}
