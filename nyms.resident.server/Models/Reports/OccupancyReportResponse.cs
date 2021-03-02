using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models.Reports
{
    public class OccupancyReportResponse
    {
        public DateTime ThisDate { get; set; }
        public int TotalNumberOfResidents { get; set; }
        public int CareHomeId { get; set; }
        public string CareHomeName { get; set; }
        public int CareHomeDivisionId { get; set; }
        public string CareHomeDivisionName { get; set; }
        public int LettableRooms { get; set; }
    }
}