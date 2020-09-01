using nyms.resident.server.Models.Base;
using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class User : IPerson
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public IEnumerable<Role> Roles { get; }
    }
}