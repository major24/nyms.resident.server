using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Invoice
{
    public abstract class ContributorBase
    {
        public string Name { get; }
/*        public DateTime BillingBeginDate { get; set; }
        public DateTime BillingEndDate { get; set; }*/

        private IEnumerable<Schedule> schedules;

        public ContributorBase(string name, IEnumerable<Schedule> schedules)
        {
            Name = name;
            this.schedules = schedules;
        }

        public IEnumerable<Schedule> GetSchedules()
        {
            return schedules;
        }
    }
}