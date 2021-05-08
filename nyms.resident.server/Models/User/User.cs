using nyms.resident.server.Models.Base;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class User : IPerson
    {
        public Guid ReferenceId { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public IEnumerable<CareHomeRole> CareHomeRoles { get; set; }
    }
}