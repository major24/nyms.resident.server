using System.Collections.Generic;

namespace nyms.resident.server.Invoice
{
    public class ResidentContributor : ContributorBase
    {
        public ResidentContributor(string name, IEnumerable<Schedule> schedules) 
            : base(name, schedules)
        {
        }
    }
}