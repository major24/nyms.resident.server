using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Invoice
{
    public class InvoiceResident
    {

        private int id { get; }
        private string name { get; }
        private DateTime residencyStartDate { get; }
        private DateTime? residencyEndDate { get; }
        ICollection<ContributorBase> contributors;


        public InvoiceResident(int id,
            string name, 
/*            DateTime residencyStartDate, 
            DateTime residencyEndDate,*/
            ICollection<ContributorBase> contributors)
        {
            this.id = id;
            this.name = name;
            this.residencyStartDate = residencyStartDate;
            this.residencyEndDate = residencyEndDate;
            this.contributors = contributors;
        }

        public ICollection<ContributorBase> GetContributors()
        {
            return contributors;
        }

        public DateTime ResidencyStartDate { get { return this.residencyStartDate; } }
        public DateTime? ResidencyEndDate { get { return this.residencyEndDate; } }




        /*
        public void AddContributor(ContributorBase contributor)
        {
            contributors.Add(contributor);
        }

*/

    }
}