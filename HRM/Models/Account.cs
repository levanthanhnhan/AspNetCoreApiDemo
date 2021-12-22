using System;
using System.Runtime.InteropServices;

namespace HRM.Models
{
    public class Account
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? StaffId { get; set; }
        public string StaffName { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string RefreshToken { get; set; }
        public DateTime TokenExpired { get; set; }
        public string OldPassword { get; set; }
    }
}
