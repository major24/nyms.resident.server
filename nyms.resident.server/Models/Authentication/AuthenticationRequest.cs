using System.ComponentModel.DataAnnotations;

namespace nyms.resident.server.Models
{
    public class AuthenticationRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}