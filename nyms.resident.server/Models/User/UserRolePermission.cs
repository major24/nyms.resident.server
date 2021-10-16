namespace nyms.resident.server.Models
{
    // DO NOT USE in the future. Instead use UserRoleAccess class
    public class UserRolePermission
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int CareHomeId { get; set; }
        public int SpendCategoryId { get; set; }
    }
}