namespace nyms.resident.server.Models
{
    public class ResidentContact
    {
        public int Id { get; set; }
        public int ResidentId { get; set; }
        public string ContactType { get; set; }
        public string Data { get; set; }
        public string Active { get; set; }
    }
}