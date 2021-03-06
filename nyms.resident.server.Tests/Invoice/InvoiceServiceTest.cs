using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Invoice;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Impl;
using nyms.resident.server.Services.Interfaces;
using System.Linq;
using NLog;
using nyms.resident.server.Services.Core;

namespace nyms.resident.server.Tests.Invoice
{
    [TestClass]
    public class InvoiceServiceTest
    {
        [TestMethod]
        public void GetInvoiceDataByDateTest()
        {
            var startDate = new DateTime(2020, 01, 01);
            var endDate = new DateTime(2020, 01, 31);
            // arrange
            Mock<IInvoiceDataProvider> mockInvDP = new Mock<IInvoiceDataProvider>();
            Mock<IResidentDataProvider> mockResDP = new Mock<IResidentDataProvider>();
            IFeeCalculatorService feeCalculatorService = new FeeCalculatorService();
            Mock<IUserService> mockUserDP = new Mock<IUserService>();
            Mock<IBillingCycleDataProvider> mockBcDP = new Mock<IBillingCycleDataProvider>();

            mockResDP.Setup(m => m.GetResidents()).Returns(GetResidents());
            mockInvDP.Setup(m => m.GetAllSchedulesForInvoiceDate(startDate, endDate)).Returns(GetSchedulePayments());
            mockUserDP.Setup(m => m.GetUsers()).Returns(GetUsers());

            IInvoiceService srv = new InvoiceService(mockInvDP.Object, mockResDP.Object, feeCalculatorService, mockUserDP.Object, mockBcDP.Object);
            var actual = srv.GetInvoiceData(startDate, endDate);

            // assert
            decimal[] amountDues = { 714.29M, 285.71M, 1200.00M, 900.00M }; // calcualted based on dates. see schedule data
            decimal wkFee = 700.00M;

            Assert.AreEqual(wkFee, actual.InvoiceResidents.FirstOrDefault().ResidentWeeklyFee);
            var sps = actual.InvoiceResidents.FirstOrDefault().SchedulePayments.ToList();
            int i = 0;
            sps.ForEach(s =>
            {
                Assert.AreEqual(amountDues[i], s.AmountDue);
                i++;
            });
        }


        private IEnumerable<Resident> GetResidents()
        {
            List<Resident> residents = new List<Resident>()
            {
                new Resident() { Id = 10, ForeName = "R1", CareHomeId = 1, CareHomeDivisionId = 1, AdmissionDate = new DateTime(2020, 01, 01), ExitDate = new DateTime(9999, 12, 31)  },
            };
            return residents.ToArray();
        }

        private IEnumerable<SchedulePayment> GetSchedulePayments()
        {
            List<SchedulePayment> list = new List<SchedulePayment>()
            {
                new SchedulePayment() {ResidentId = 10, LocalAuthorityId = 1, PaymentProviderId = 1, PaymentTypeId = 1,
                    ScheduleBeginDate = new DateTime(2020, 01, 01), ScheduleEndDate = new DateTime(2020, 01, 10), WeeklyFee = 500.00M },
                new SchedulePayment() {ResidentId = 10, LocalAuthorityId = 1, PaymentProviderId = 1, PaymentTypeId = 2,
                    ScheduleBeginDate = new DateTime(2020, 01, 01), ScheduleEndDate = new DateTime(2020, 01, 10), WeeklyFee = 200.00M },
                new SchedulePayment() {ResidentId = 10, LocalAuthorityId = 1, PaymentProviderId = 1, PaymentTypeId = 1,
                    ScheduleBeginDate = new DateTime(2020, 01, 11), ScheduleEndDate = new DateTime(9999, 01, 01), WeeklyFee = 400.00M },
                new SchedulePayment() {ResidentId = 10, LocalAuthorityId = 1, PaymentProviderId = 1, PaymentTypeId = 2,
                    ScheduleBeginDate = new DateTime(2020, 01, 11), ScheduleEndDate = new DateTime(9999, 01, 01), WeeklyFee = 300.00M },
            };
            return list.ToArray();
        }



        private IEnumerable<User> GetUsers()
        {
            return  new List<User>()
            {
                new User() { Id = 1, ForeName = "Major", SurName = "Nalliah" }
            }.ToArray();
        }
    }
}
