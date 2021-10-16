using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class UserRoleAccess
    {
        public int UserId { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public int RoleId { get; set; }
        public int CareHomeId { get; set; }
        public string RoleName { get; set; }
    }
}