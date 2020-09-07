using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Invoice
{
    public class InvoiceResident
    {

        public int Id { get; set; }
        public string Name { get; }
        public decimal TotalLaFee { get; set; }
        public decimal ResidentWeeklyFee { get; set; }
        public decimal GrandTotal { get; set; }
        public int? LocalAuthorityId { get; set; }
        public IEnumerable<Schedule> Schedules { get; set; }

        public InvoiceResident(int id, string name, IEnumerable<Schedule> schedules)
        {
            this.Id = id;
            this.Name = name;
            this.Schedules = schedules;
        }

        public IEnumerable<Schedule> GetSchedules()
        {
            return Schedules;
        }
        public void SetSchedules(IEnumerable<Schedule> schedules)
        {
            this.Schedules = schedules;
        }
    }
}