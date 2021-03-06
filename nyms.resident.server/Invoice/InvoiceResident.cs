using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Invoice
{
    public class InvoiceResident
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal TotalLaFee { get; set; }
        public decimal ResidentWeeklyFee { get; set; }
        public decimal GrandTotal { get; set; }
        public int? LocalAuthorityId { get; set; }
        public int CareHomeDivisionId { get; set; }
        public string CareHomeDivisionName { get; set; }
        public string LocalAuthorityName { get; set; }
        public IEnumerable<SchedulePayment> SchedulePayments { get; set; }

        public InvoiceResident(int id, string name, IEnumerable<SchedulePayment> schedulePayments)
        {
            this.Id = id;
            this.Name = name;
            this.SchedulePayments = schedulePayments;
        }

        public IEnumerable<SchedulePayment> GetSchedules()
        {
            return SchedulePayments;
        }
        public void SetSchedules(IEnumerable<SchedulePayment> schedulePayments)
        {
            this.SchedulePayments = schedulePayments;
        }
    }
}