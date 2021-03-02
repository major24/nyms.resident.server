using nyms.resident.server.Models.Reports;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IReportService
    {
        IEnumerable<OccupancyReportResponse> GetOccupancyReport(DateTime startDate, DateTime endDate);
    }
}