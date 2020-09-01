namespace nyms.resident.server.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int RoomLocationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}