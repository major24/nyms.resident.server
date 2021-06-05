using System;

namespace nyms.resident.server.Models
{
    public class SpendRequest
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public decimal Amount { get; set; }
        public string PoNumber { get; set; }
        public string Notes { get; set; }
        public string TranType { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}