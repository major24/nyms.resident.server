using Microsoft.Ajax.Utilities;
using nyms.resident.server.DataProviders.Interfaces;
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
            if (!residents.Any()) return null;

            List<OccupancyReportResponse> listResult = new List<OccupancyReportResponse>();
            // find by divisions
            var divisionIds= residents.Select(r => r.CareHomeDivisionId).Distinct();
            divisionIds.ForEach(divId =>
            {
                List<OccupancyCountByDate> list = new List<OccupancyCountByDate>();
                var divName = residents.Where(r => r.CareHomeDivisionId == divId).FirstOrDefault().CareHomeDivisionName;
                listOfAllDates.ForEach((thisDate) =>
                {
                    var listOfResidentStayedForTheDate = residents
                        .FindAll(r => r.AdmissionDate <= thisDate && r.DischargedFromHomeDate >= thisDate.AddDays(1))
                        .Where(r => r.CareHomeDivisionId == divId);
                    OccupancyCountByDate occupancyCountByDate = new OccupancyCountByDate()
                    {
                        ThisDate = thisDate,
                        TotalNumberOfResidents = listOfResidentStayedForTheDate.Count()
                    };
                    list.Add(occupancyCountByDate);
                });
                OccupancyReportResponse occupancyReportResponse = new OccupancyReportResponse(divId)
                {
                    GroupBy = "Division",
                    Name = divName,
                    OccupancyCountByDates = list
                };
                listResult.Add(occupancyReportResponse);
            });

            // get counts for fund provider ids
            var fundProviderIds = residents.Select(r => r.LocalAuthorityId).Distinct();
            fundProviderIds.ForEach(provId =>
            {
                List<OccupancyCountByDate> list = new List<OccupancyCountByDate>();
                var provName = residents.Where(r => r.LocalAuthorityId == provId).FirstOrDefault().LocalAuthorityName;
                listOfAllDates.ForEach((thisDate) =>
                {
                    var listOfResidentStayedForTheDate = residents
                        .FindAll(r => r.AdmissionDate <= thisDate && r.DischargedFromHomeDate >= thisDate.AddDays(1))
                        .Where(r => r.LocalAuthorityId == provId);
                    OccupancyCountByDate occupancyCountByDate = new OccupancyCountByDate()
                    {
                        ThisDate = thisDate,
                        TotalNumberOfResidents = listOfResidentStayedForTheDate.Count()
                    };
                    list.Add(occupancyCountByDate);
                });
                OccupancyReportResponse occupancyReportResponse = new OccupancyReportResponse()
                {
                    GroupBy = "FundProvider",
                    Name = provName,
                    OccupancyCountByDates = list
                };
                listResult.Add(occupancyReportResponse);
            });

            return listResult.ToArray();
        }
    }
}