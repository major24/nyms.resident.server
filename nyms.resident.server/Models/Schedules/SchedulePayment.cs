using nyms.resident.server.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Models
{
    public class SchedulePayment
    {
        public SchedulePayment()
        {
            LocalAuthorityId = null;
        }
        public int Id { get; set; }
        public int ResidentId { get; set; }
        public int? LocalAuthorityId { get; set; }
        public int PaymentTypeId { get; set; }
        public int PaymentProviderId { get; set; }
        public string PaymentFromName { get; set; }
        public string Description { get; set; }
        public DateTime ScheduleBeginDate { get; set; }
        public DateTime ScheduleEndDate { get; set; }
        public decimal WeeklyFee { get; set; }
        public decimal AmountDue { get; set; }
        public int NumberOfDays { get; set; }
        /*public InvoiceValidatedEntity InvoiceValidatedEntity { get; set; }*/
        public InvoiceValidatedModel InvoiceValidatedModel { get; set; }
        public string[] Comments { get; set; }

/*        public string Validated { get; set; }
        public decimal ValidatedAmount { get; set; }
        public string ValidatedBy { get; set; }
        public decimal TransactionAmount { get; set; }
        public string Comments { get; set; } 
        public DateTime UpdatedDate { get; set; }*/
    }
}