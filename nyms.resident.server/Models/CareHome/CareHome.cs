
using System;
using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class CareHome
    {
        public int Id { get; set; }
        public Guid ReferenceId { get; set; }
        public string Name { get; set; }
        public IEnumerable<RoomLocation> RoomLocations { get; set; }
        public IEnumerable<CareCategory> CareCategories { get; set; }
        public IEnumerable<LocalAuthority> LocalAuthorities { get; set; }
    }
}