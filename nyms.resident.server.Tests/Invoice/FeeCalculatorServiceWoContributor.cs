using Microsoft.VisualStudio.TestTools.UnitTesting;
using nyms.resident.server.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nyms.resident.server.Tests.Invoice
{
    [TestClass]
    public class FeeCalculatorServiceWoContributor
    {
        [TestMethod]
        public void CalculateFeeLaFullPeriodNoFeeChange()
        {
            FeeCalculatorService srv = new FeeCalculatorService();

            // get schedules for John
            var schedules = getSchedules(10);
            var invoiceResident = new InvoiceResident(10, "John", schedules);

            var resident = srv.CalculateFee(invoiceResident, new DateTime(2020, 01, 01), new DateTime(2020, 01, 28));

            var feesPerSchedules = resident.GetSchedules();
            var amounts = feesPerSchedules.Select(c => c.AmountDue).ToArray();
            var totalAmount = amounts.Sum();

            // Fee = 599.99M 
            // For 28 days: 599.99 / 7 * 28 = 2399.96
            Assert.AreEqual(2399.96M, totalAmount);
        }

        [TestMethod]
        public void CalculateFeeLaAndCcFullPeriodNoFeeChange()
        {
            FeeCalculatorService srv = new FeeCalculatorService();

            var schedules = getSchedules(11);
            var invoiceResident = new InvoiceResident(11, "John", schedules);

            var resident = srv.CalculateFee(invoiceResident, new DateTime(2020, 01, 01), new DateTime(2020, 01, 28));

            var feesPerSchedules = resident.GetSchedules();
            var amounts = feesPerSchedules.Select(c => c.AmountDue).ToArray();
            var totalAmount = amounts.Sum();

            // Fee = la= 555.75M  cc= 245.43M
            // For 28 days: 555.75+245.43 / 7 * 28 = 3204.72
            Assert.AreEqual(3204.72M, totalAmount);
        }


        [TestMethod]
        public void CalculateFeeLaResidentLeavesMidMonth()
        {
            FeeCalculatorService srv = new FeeCalculatorService();

            var schedules = getSchedules(20);
            var invoiceResident = new InvoiceResident(20, "John", schedules);

            var resident = srv.CalculateFee(invoiceResident, new DateTime(2020, 01, 01), new DateTime(2020, 01, 28));

            var feesPerSchedules = resident.GetSchedules();
            var amounts = feesPerSchedules.Select(c => c.AmountDue).ToArray();
            var totalAmount = amounts.Sum();

            // Fee = la= 680.10 
            // For 15 days: 1457.36
            Assert.AreEqual(1457.36M, totalAmount);
        }

        [TestMethod]
        public void CalculateFeeLaResidentArrivesMidMonth()
        {
            FeeCalculatorService srv = new FeeCalculatorService();

            var schedules = getSchedules(21);
            var invoiceResident = new InvoiceResident(21, "John", schedules);

            var resident = srv.CalculateFee(invoiceResident, new DateTime(2020, 01, 01), new DateTime(2020, 01, 28));

            var feesPerSchedules = resident.GetSchedules();
            var amounts = feesPerSchedules.Select(c => c.AmountDue).ToArray();
            var totalAmount = amounts.Sum();

            // Fee = la= 678.01 
            // For 8 days: 774.87
            Assert.AreEqual(774.87M, totalAmount);
        }

        // *** Fee changes
        [TestMethod]
        public void CalculateFeeLaFullPeriodWithFeeChange()
        {
            FeeCalculatorService srv = new FeeCalculatorService();

            var schedules = getSchedules(30);
            var invoiceResident = new InvoiceResident(30, "John", schedules);

            var resident = srv.CalculateFee(invoiceResident, new DateTime(2020, 01, 01), new DateTime(2020, 01, 28));

            var feesPerSchedules = resident.GetSchedules();
            var amounts = feesPerSchedules.Select(c => c.AmountDue).ToArray();
            var totalAmount = amounts.Sum();

            // (30, 1, "LA", new DateTime(2019, 01, 01), new DateTime(2020, 01, 15), 630.50M));
            // (30, 1, "LA", new DateTime(2020, 01, 16), new DateTime(2050, 12, 31), 655.75M));
            // For 630.50 for 15 days = 1351.07
            // For 655.75 for 13 days = 1217.82  = 2568.89
            Assert.AreEqual(2568.89M, totalAmount);
        }

        [TestMethod]
        public void CalculateFeeLaFullPeriodWithFeeChangeMultipleChgInOnePeriod()
        {
            FeeCalculatorService srv = new FeeCalculatorService();

            var schedules = getSchedules(40);
            var invoiceResident = new InvoiceResident(40, "John", schedules);

            var resident = srv.CalculateFee(invoiceResident, new DateTime(2020, 01, 01), new DateTime(2020, 01, 28));

            var feesPerSchedules = resident.GetSchedules();
            var amounts = feesPerSchedules.Select(c => c.AmountDue).ToArray();
            var totalAmount = amounts.Sum();

            // Multiple Fee changes
            // (40, 1, "LA", new DateTime(2019, 01, 01), new DateTime(2020, 01, 10), 600.50M));
            // (40, 1, "LA", new DateTime(2020, 01, 11), new DateTime(2020, 01, 15), 630.90M));
            // (40, 1, "LA", new DateTime(2020, 01, 16), new DateTime(2050, 12, 31), 250.50M));
            // (40, 99, "CC", new DateTime(2019, 01, 01), new DateTime(2020, 01, 10), 150.44M));
            // (40, 99, "CC", new DateTime(2020, 01, 11), new DateTime(2020, 01, 15), 165.78M));
            // (40, 99, "CC", new DateTime(2020, 01, 16), new DateTime(2050, 12, 31), 550.58M));

            // LA 600.50    CC 150.44  for 10 days   
            // LA 630.90    CC 165.78  for  5 days
            // LA 250.50    CC 550.58  for 13 days  28 days

            Assert.AreEqual(3129.54M, totalAmount);
        }

        // Longer period than schecue
        [TestMethod]
        public void CalculateFeeLaFullPeriodNoFeeChangeForCustomDates()
        {
            FeeCalculatorService srv = new FeeCalculatorService();

            var schedules = getSchedules(10);
            var invoiceResident = new InvoiceResident(10, "John", schedules);

            var resident = srv.CalculateFee(invoiceResident, new DateTime(2020, 01, 01), new DateTime(2020, 03, 31)); // <<--quarter

            var feesPerSchedules = resident.GetSchedules();
            var amounts = feesPerSchedules.Select(c => c.AmountDue).ToArray();
            var totalAmount = amounts.Sum();

            // Fee = 599.99M 
            // For 91 days: 599.99 / 7 * 91 = 7799.87
            Assert.AreEqual(7799.87M, totalAmount);
        }

        [TestMethod]
        public void CalculateFeeLaFullPeriodWithFeeChangeMultipleChgInOnePeriodForCustomDates()
        {
            FeeCalculatorService srv = new FeeCalculatorService();

            var schedules = getSchedules(40);
            var invoiceResident = new InvoiceResident(40, "John", schedules);

            var resident = srv.CalculateFee(invoiceResident, new DateTime(2020, 01, 01), new DateTime(2020, 03, 31));

            var feesPerSchedules = resident.GetSchedules();
            var amounts = feesPerSchedules.Select(c => c.AmountDue).ToArray();
            var totalAmount = amounts.Sum();

            // Multiple Fee changes
            // (40, 1, "LA", new DateTime(2019, 01, 01), new DateTime(2020, 01, 10), 600.50M));
            // (40, 1, "LA", new DateTime(2020, 01, 11), new DateTime(2020, 01, 15), 630.90M));
            // (40, 1, "LA", new DateTime(2020, 01, 16), new DateTime(2050, 12, 31), 250.50M));
            // (40, 99, "CC", new DateTime(2019, 01, 01), new DateTime(2020, 01, 10), 150.44M));
            // (40, 99, "CC", new DateTime(2020, 01, 11), new DateTime(2020, 01, 15), 165.78M));
            // (40, 99, "CC", new DateTime(2020, 01, 16), new DateTime(2050, 12, 31), 550.58M));

            // LA 600.50    CC 150.44  for 10 days   
            // LA 630.90    CC 165.78  for  5 days
            // LA 250.50    CC 550.58  for 13 days  91 days

            Assert.AreEqual(10339.26M, totalAmount);
        }

        [TestMethod]
        public void CalculateFeeLaAndCcWithLaMultiplePaymentsAndCcMultiplePayments()
        {
            FeeCalculatorService srv = new FeeCalculatorService();

            var schedules = getSchedules(50);
            var invoiceResident = new InvoiceResident(50, "John", schedules);

            var resident = srv.CalculateFee(invoiceResident, new DateTime(2020, 01, 01), new DateTime(2020, 01, 28));

            var feesPerSchedules = resident.GetSchedules();
            var amounts = feesPerSchedules.Select(c => c.AmountDue).ToArray();
            var totalAmount = amounts.Sum();

            // multiple payments categories from la
            // (50, 1, "LA", new DateTime(2019, 01, 01), new DateTime(2050, 12, 31), 499.99M));
            // (50, 1, "LA", new DateTime(2019, 01, 01), new DateTime(2050, 12, 31), 69.75M));
            // (50, 1, "LA", new DateTime(2019, 01, 01), new DateTime(2050, 12, 31), 25.55M));
            // (50, 99, "CC", new DateTime(2019, 01, 01), new DateTime(2050, 12, 31), 125.75M));
            // (50, 99, "CC", new DateTime(2019, 01, 01), new DateTime(2050, 12, 31), 13.33M));


            // For 28 days: all totals = 2937.48
            Assert.AreEqual(2937.48M, totalAmount);
        }



        private IEnumerable<Schedule> getSchedules(int residentId)
        {
            // ResId, LaId, ContributorName, schBeginDate, schEndDate
            List<Tuple<int, int, string, DateTime, DateTime, decimal>> allSchedules = new List<Tuple<int, int, string, DateTime, DateTime, decimal>>();

            // only la. one valid period
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (10, 1, "LA", new DateTime(2018, 01, 01), new DateTime(2018, 12, 31), 400.00M));
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (10, 1, "LA", new DateTime(2019, 01, 01), new DateTime(2050, 12, 31), 599.99M));

            // la and cc
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (11, 1, "LA", new DateTime(2019, 01, 01), new DateTime(2050, 12, 31), 555.75M));
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (11, -99, "CC", new DateTime(2019, 01, 01), new DateTime(2050, 12, 31), 245.43M));


            // resi leave mid month
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (20, 1, "LA", new DateTime(2019, 01, 01), new DateTime(2020, 01, 15), 680.10M));

            // resi arrives mid month
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (21, 1, "LA", new DateTime(2020, 01, 21), new DateTime(2050, 12, 31), 678.01M));


            // Fee change
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (30, 1, "LA", new DateTime(2019, 01, 01), new DateTime(2020, 01, 15), 630.50M));
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (30, 1, "LA", new DateTime(2020, 01, 16), new DateTime(2050, 12, 31), 655.75M));


            // Multiple Fee changes
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (40, 1, "LA", new DateTime(2019, 01, 01), new DateTime(2020, 01, 10), 600.50M));
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (40, 1, "LA", new DateTime(2020, 01, 11), new DateTime(2020, 01, 15), 630.90M));
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (40, 1, "LA", new DateTime(2020, 01, 16), new DateTime(2050, 12, 31), 250.50M));
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (40, 99, "CC", new DateTime(2019, 01, 01), new DateTime(2020, 01, 10), 150.44M));
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (40, 99, "CC", new DateTime(2020, 01, 11), new DateTime(2020, 01, 15), 165.78M));
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (40, 99, "CC", new DateTime(2020, 01, 16), new DateTime(2050, 12, 31), 550.58M));


            // multiple payments categories from la
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (50, 1, "LA", new DateTime(2019, 01, 01), new DateTime(2050, 12, 31), 499.99M));
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (50, 1, "LA", new DateTime(2019, 01, 01), new DateTime(2050, 12, 31), 69.75M));
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (50, 1, "LA", new DateTime(2019, 01, 01), new DateTime(2050, 12, 31), 25.55M));
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (50, 99, "CC", new DateTime(2019, 01, 01), new DateTime(2050, 12, 31), 125.75M));
            allSchedules.Add(new Tuple<int, int, string, DateTime, DateTime, decimal>
                (50, 99, "CC", new DateTime(2019, 01, 01), new DateTime(2050, 12, 31), 13.33M));

            var list = allSchedules.Where(s => s.Item1 == residentId);
            var schs = list.Select(s =>
            {
                return new Schedule() { LocalAuthorityId = s.Item2, PaymentFrom = s.Item3, ScheduleBeginDate = s.Item4, ScheduleEndDate = s.Item5, WeeklyFee = s.Item6 };
            });

            return schs.ToArray();
        }


    }
}
