using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models.Reports
{
    public class OccupancyReportResponse
    {
        Dictionary<int, int> LettableRoomsLookup = new Dictionary<int, int>()
        {
            { 1, 28 },
            { 2, 25 }
        };

        public OccupancyReportResponse()
        {
        }

        public OccupancyReportResponse(int divisionId)
        {
            if (divisionId <= 0) throw new ArgumentException(nameof(divisionId));
            LettableRooms = LettableRoomsLookup[divisionId];
        }

        public string GroupBy { get; set; }
        public string Name { get; set; }
        public int LettableRooms { get; }
        public List<OccupancyCountByDate> OccupancyCountByDates = new List<OccupancyCountByDate>();
    }

    public class OccupancyCountByDate
    {
        public DateTime ThisDate { get; set; }
        public int TotalNumberOfResidents { get; set; }
    }
}