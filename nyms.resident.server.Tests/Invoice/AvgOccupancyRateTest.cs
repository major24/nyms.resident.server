using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Models.Reports;
using nyms.resident.server.Services.Impl;
using nyms.resident.server.Services.Interfaces;

namespace nyms.resident.server.Tests.Invoice
{
    [TestClass]
    public class AverageOccupancyTest
    {
        [TestMethod]
        public void OccupancyCalucateByDateByDivisionTest()
        {
            List<Resident> residents = new List<Resident>()
            {
                new Resident() { ForeName = "R1", CareHomeId = 1, CareHomeDivisionId = 1, AdmissionDate = new DateTime(2020, 01, 15), DischargedFromHomeDate = new DateTime(9999, 12, 31), CareHomeDivisionName = "PC", LocalAuthorityId = 1, LocalAuthorityName = "Derby" },
                new Resident() { ForeName = "R2", CareHomeId = 1, CareHomeDivisionId = 1, AdmissionDate = new DateTime(2020, 01, 16), DischargedFromHomeDate = new DateTime(2020, 01, 20), CareHomeDivisionName = "PC", LocalAuthorityId = 1, LocalAuthorityName = "Derby" },
                new Resident() { ForeName = "R3", CareHomeId = 1, CareHomeDivisionId = 2, AdmissionDate = new DateTime(2020, 01, 16), DischargedFromHomeDate = new DateTime(9999, 12, 31), CareHomeDivisionName = "ML", LocalAuthorityId = 1, LocalAuthorityName = "Derby" },
                new Resident() { ForeName = "R3", CareHomeId = 1, CareHomeDivisionId = 2, AdmissionDate = new DateTime(2020, 01, 18), DischargedFromHomeDate = new DateTime(9999, 12, 31), CareHomeDivisionName = "ML", LocalAuthorityId = 1, LocalAuthorityName = "Derby" },
                new Resident() { ForeName = "R3", CareHomeId = 1, CareHomeDivisionId = 1, AdmissionDate = new DateTime(2020, 01, 19), DischargedFromHomeDate = new DateTime(9999, 12, 31), CareHomeDivisionName = "PC", LocalAuthorityId = 2, LocalAuthorityName = "Manchester" },
                new Resident() { ForeName = "R4", CareHomeId = 1, CareHomeDivisionId = 2, AdmissionDate = new DateTime(2020, 01, 22), DischargedFromHomeDate = new DateTime(9999, 12, 31), CareHomeDivisionName = "ML", LocalAuthorityId = 2, LocalAuthorityName = "Manchester" },
                new Resident() { ForeName = "R5", CareHomeId = 1, CareHomeDivisionId = 1, AdmissionDate = new DateTime(2020, 01, 23), DischargedFromHomeDate = new DateTime(2020, 01, 25), CareHomeDivisionName = "PC", LocalAuthorityId = 100, LocalAuthorityName = "Private" },
            };
            var startDate = new DateTime(2020, 01, 14);
            var endDate = new DateTime(2020, 01, 26);

            // arrange
            Mock<IResidentContactDataProvider> mockResContactsDP = new Mock<IResidentContactDataProvider>();
            Mock<ISocialWorkerDataProvider> mockSwDP = new Mock<ISocialWorkerDataProvider>();
            Mock<IResidentDataProvider> mockResDP = new Mock<IResidentDataProvider>();

            mockResDP.Setup(m => m.GetResidents()).Returns(residents.ToArray());

            // act
            IReportService reportService = new ReportService(mockResDP.Object);
            IEnumerable<OccupancyReportResponse> result = reportService.GetOccupancyReport(startDate, endDate);

            List<OccupancyReportResponse> listResult = result.ToList(); // req for below test only
            // assert
            List<OccupancyCountByDate> expectedPC = new List<OccupancyCountByDate>()
            {
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 14), TotalNumberOfResidents = 0 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 15), TotalNumberOfResidents = 1 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 16), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 17), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 18), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 19), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 20), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 21), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 22), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 23), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 24), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 25), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 26), TotalNumberOfResidents = 2 },

            };

            List<OccupancyCountByDate> expectedML = new List<OccupancyCountByDate>()
            {
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 14), TotalNumberOfResidents = 0 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 15), TotalNumberOfResidents = 0 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 16), TotalNumberOfResidents = 1 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 17), TotalNumberOfResidents = 1 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 18), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 19), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 20), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 21), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 22), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 23), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 24), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 25), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 26), TotalNumberOfResidents = 3 },
            };

            List<OccupancyCountByDate> expectedDerby = new List<OccupancyCountByDate>()
            {
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 14), TotalNumberOfResidents = 0 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 15), TotalNumberOfResidents = 1 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 16), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 17), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 18), TotalNumberOfResidents = 4 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 19), TotalNumberOfResidents = 4 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 20), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 21), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 22), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 23), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 24), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 25), TotalNumberOfResidents = 3 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 26), TotalNumberOfResidents = 3 },
            };

            List<OccupancyCountByDate> expectedManchester = new List<OccupancyCountByDate>()
            {
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 14), TotalNumberOfResidents = 0 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 15), TotalNumberOfResidents = 0 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 16), TotalNumberOfResidents = 0 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 17), TotalNumberOfResidents = 0 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 18), TotalNumberOfResidents = 0 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 19), TotalNumberOfResidents = 1 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 20), TotalNumberOfResidents = 1 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 21), TotalNumberOfResidents = 1 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 22), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 23), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 24), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 25), TotalNumberOfResidents = 2 },
                new OccupancyCountByDate() { ThisDate = new DateTime(2020, 01, 26), TotalNumberOfResidents = 2 },
            };
            var actPC = listResult.Where(actResult => actResult.Name == "PC").FirstOrDefault();
            var actML = listResult.Where(actResult => actResult.Name == "ML").FirstOrDefault();

            int i = 0;
            actPC.OccupancyCountByDates.ForEach(act =>
            {
                Assert.AreEqual(expectedPC[i].TotalNumberOfResidents, act.TotalNumberOfResidents);
                i++;
            });

            i = 0;
            actML.OccupancyCountByDates.ForEach(act =>
            {
                Assert.AreEqual(expectedML[i].TotalNumberOfResidents, act.TotalNumberOfResidents);
                i++;
            });

            var actDerby = listResult.Where(actResult => actResult.Name == "Derby").FirstOrDefault();
            var actManchester = listResult.Where(actResult => actResult.Name == "Manchester").FirstOrDefault();
            i = 0;
            actDerby.OccupancyCountByDates.ForEach(act =>
            {
                Assert.AreEqual(expectedDerby[i].TotalNumberOfResidents, act.TotalNumberOfResidents);
                i++;
            });

            i = 0;
            actManchester.OccupancyCountByDates.ForEach(act =>
            {
                Assert.AreEqual(expectedManchester[i].TotalNumberOfResidents, act.TotalNumberOfResidents);
                i++;
            });

            
            /*List<OccupancyReportResponse> expected = new List<OccupancyReportResponse>()
            {
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 13), CareHomeDivisionId = 0, TotalNumberOfResidents = 0 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 14), CareHomeDivisionId = 0, TotalNumberOfResidents = 0 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 15), CareHomeDivisionId = 1, TotalNumberOfResidents = 1 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 16), CareHomeDivisionId = 1, TotalNumberOfResidents = 2 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 16), CareHomeDivisionId = 2, TotalNumberOfResidents = 1 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 17), CareHomeDivisionId = 1, TotalNumberOfResidents = 2 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 17), CareHomeDivisionId = 2, TotalNumberOfResidents = 1 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 18), CareHomeDivisionId = 1, TotalNumberOfResidents = 2 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 18), CareHomeDivisionId = 2, TotalNumberOfResidents = 2 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 19), CareHomeDivisionId = 1, TotalNumberOfResidents = 3 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 19), CareHomeDivisionId = 2, TotalNumberOfResidents = 2 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 20), CareHomeDivisionId = 1, TotalNumberOfResidents = 2 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 20), CareHomeDivisionId = 2, TotalNumberOfResidents = 2 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 21), CareHomeDivisionId = 1, TotalNumberOfResidents = 2 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 21), CareHomeDivisionId = 2, TotalNumberOfResidents = 2 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 22), CareHomeDivisionId = 1, TotalNumberOfResidents = 2 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 22), CareHomeDivisionId = 2, TotalNumberOfResidents = 3 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 23), CareHomeDivisionId = 1, TotalNumberOfResidents = 3 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 23), CareHomeDivisionId = 2, TotalNumberOfResidents = 3 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 24), CareHomeDivisionId = 1, TotalNumberOfResidents = 3 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 24), CareHomeDivisionId = 2, TotalNumberOfResidents = 3 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 25), CareHomeDivisionId = 1, TotalNumberOfResidents = 2 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 25), CareHomeDivisionId = 2, TotalNumberOfResidents = 3 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 26), CareHomeDivisionId = 1, TotalNumberOfResidents = 2 },
                new OccupancyReportResponse() { ThisDate = new DateTime(2020, 01, 26), CareHomeDivisionId = 2, TotalNumberOfResidents = 3 },
            };
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].ThisDate, listResult[i].ThisDate);
                Assert.AreEqual(expected[i].TotalNumberOfResidents, listResult[i].TotalNumberOfResidents);
                Assert.AreEqual(expected[i].CareHomeDivisionId, listResult[i].CareHomeDivisionId);
            }*/
        }


    }
}
