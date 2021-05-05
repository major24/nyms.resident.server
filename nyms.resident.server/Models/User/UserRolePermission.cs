namespace nyms.resident.server.Models
{
    public class UserRolePermission
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int CareHomeId { get; set; }
        public int SpendCategoryId { get; set; }
    }
}