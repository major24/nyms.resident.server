using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Invoice;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Reports;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace nyms.resident.server.Services.Impl
{
    public class ReportService : IReportService
    {
        private readonly IResidentDataProvider _residentDataProvider;

        public ReportService(IResidentDataProvider residentDataProvider)
        {
            _residentDataProvider = residentDataProvider ?? throw new ArgumentException(nameof(residentDataProvider));
        }

        public IEnumerable<OccupancyReportResponse> GetOccupancyReport(DateTime startDate, DateTime endDate)
        {
            // Get how many days we need to calculate
            var totalNumOfDays = (int)(endDate - startDate).TotalDays + 1;
            // Get each days from start and end date
            var listOfAllDates = Enumerable.Range(0, totalNumOfDays).Select(day => startDate.AddDays(day)).ToList();

            var residents = _residentDataProvider.GetResidents().ToList();
            if (!residents.Any())
            {
                return null;
            }

            List<OccupancyReportResponse> listResult = new List<OccupancyReportResponse>();
            listOfAllDates.ForEach((thisDate) =>
            {
                var listOfResidentStayedForTheDate = residents
                    .FindAll(r => r.AdmissionDate <= thisDate && r.ExitDate >= thisDate.AddDays(1))
                    .GroupBy(r => r.CareHomeDivisionId).ToList();
                if (listOfResidentStayedForTheDate.Any())
                {
                    foreach (IGrouping<int, Resident> grp in listOfResidentStayedForTheDate)
                    {
                        var key = grp.Key;
                        var totalResidents = grp.Count();
                        var careHomeDivisionId = grp.FirstOrDefault().CareHomeDivisionId;
                        OccupancyReportResponse result = new OccupancyReportResponse() 
                        { 
                            ThisDate = thisDate, 
                            TotalNumberOfResidents = totalResidents, 
                            CareHomeDivisionId = careHomeDivisionId 
                        };
                        listResult.Add(result);
                    }
                }
                else
                {
                    OccupancyReportResponse result = new OccupancyReportResponse() 
                    { 
                        ThisDate = thisDate, 
                        TotalNumberOfResidents = 0, 
                        CareHomeDivisionId = 0 
                    };
                    listResult.Add(result);
                }
            });

            return listResult.ToArray();
        }
    }
}