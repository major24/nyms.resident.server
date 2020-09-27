using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace nyms.resident.server.Models.Authentication
{
    public class SecurityPrincipal : System.Security.Principal.IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public int Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string ForeName { get; set; }
        public string SurName { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}