using nyms.resident.server.Models;
using System;
using System.Linq;

namespace nyms.resident.server.Invoice
{
    public class FeeCalculatorService : IFeeCalculatorService
    {
        public InvoiceResident CalculateFee(InvoiceResident resident, DateTime reportBeginDate, DateTime reportEndDate)
        {
            if (resident == null)
                throw new ArgumentNullException(nameof(resident));
            if (reportBeginDate == DateTime.MinValue || reportEndDate == DateTime.MinValue)
                throw new ArgumentOutOfRangeException("Error: Invalid date");

            var schedules = resident.GetSchedules();
            if (!schedules.Any())
                return resident;

            foreach (SchedulePayment schedule in schedules)
            {
                var isActiveSchedule = IsActiveSchedule(schedule.ScheduleBeginDate, schedule.ScheduleEndDate, reportBeginDate, reportEndDate);

                if (isActiveSchedule)
                {
                    // init var to be used in calc. NO modifiction to report date came in
                    DateTime reportBeginDateForCalc = reportBeginDate;
                    DateTime reportEndDateForCalc = reportEndDate;

                    if (schedule.ScheduleBeginDate > reportBeginDate)
                        reportBeginDateForCalc = schedule.ScheduleBeginDate;

                    // Is Residnet leaves in mid reporting period? then endReportDate should be res. exit date
                    if (schedule.ScheduleEndDate < reportEndDate)
                        reportEndDateForCalc = (DateTime)schedule.ScheduleEndDate;

                    int numberOfDays = GetNumberOfDaysInMonth(reportBeginDateForCalc, reportEndDateForCalc);
                        
                    var fee = CalculateFee(schedule.WeeklyFee, numberOfDays);
                    schedule.AmountDue = fee;
                    schedule.NumberOfDays = numberOfDays;
                }
            }

            return resident;
        }

        private bool IsActiveSchedule(DateTime scheduleBeginDate, DateTime scheduleEndDate, DateTime reportBeginDate, DateTime reportEndDate)
        {
            // ensure schedule is not expired
            if (scheduleEndDate < reportBeginDate)
            {
                return false;
            }
            // ensure schedule is not started
            if (scheduleBeginDate > reportEndDate)
            {
                return false;
            }
            return true;
        }

        private int GetNumberOfDaysInMonth(DateTime beginDate, DateTime endDate)
        {
            DateTime start = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day);
            DateTime end = new DateTime(endDate.Year, endDate.Month, endDate.Day);

            TimeSpan ts = end - start;

            if (ts.Days < 0)
            {
                throw new ArgumentOutOfRangeException("End date must be higher than than start date.");
            }

            return ts.Days + 1;
        }

        private decimal CalculateFee(decimal agreedWeeklyFee, int numberOfDays)
        {
            //if (agreedWeeklyFee <= 0)
              //  throw new ArgumentNullException(nameof(agreedWeeklyFee));
            if (numberOfDays <= 0)
                throw new ArgumentNullException(nameof(numberOfDays));

            // 500 / 7 = Amount per day
            // amount per day * number of days 
            return Math.Round((agreedWeeklyFee / 7 * numberOfDays), 2);
        }

    }
}

