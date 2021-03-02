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
                new Resident() { ForeName = "R1", CareHomeId = 1, CareHomeDivisionId = 1, AdmissionDate = new DateTime(2020, 01, 15), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R2", CareHomeId = 1, CareHomeDivisionId = 1, AdmissionDate = new DateTime(2020, 01, 16), ExitDate = new DateTime(2020, 01, 20)  },
                new Resident() { ForeName = "R3", CareHomeId = 1, CareHomeDivisionId = 2, AdmissionDate = new DateTime(2020, 01, 16), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R3", CareHomeId = 1, CareHomeDivisionId = 2, AdmissionDate = new DateTime(2020, 01, 18), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R3", CareHomeId = 1, CareHomeDivisionId = 1, AdmissionDate = new DateTime(2020, 01, 19), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R4", CareHomeId = 1, CareHomeDivisionId = 2, AdmissionDate = new DateTime(2020, 01, 22), ExitDate = new DateTime(9999, 12, 31)  },
                new Resident() { ForeName = "R5", CareHomeId = 1, CareHomeDivisionId = 1, AdmissionDate = new DateTime(2020, 01, 23), ExitDate = new DateTime(2020, 01, 25)  },
            };
            var startDate = new DateTime(2020, 01, 13);
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
            List<OccupancyReportResponse> expected = new List<OccupancyReportResponse>()
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
            }
        }


    }
}
