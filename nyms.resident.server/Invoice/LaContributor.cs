using System.Collections.Generic;

namespace nyms.resident.server.Invoice
{
    public class LaContributor : ContributorBase
    {
        public LaContributor(string name, IEnumerable<Schedule> schedules)
            : base(name, schedules)
        {
        }
    }
}