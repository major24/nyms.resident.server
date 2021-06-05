using System;

namespace nyms.resident.server.Models
{
    public class BudgetAllocationEntity
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public Decimal Amount { get; set; }
        public string Approved { get; set; }
        public int? ApprovedById { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int UpdatedById { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}