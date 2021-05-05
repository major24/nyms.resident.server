using System;

namespace nyms.resident.server.Models
{
    public class SpendRequest
    {
        public int Id { get; set; }
        public int SpendBudgetId { get; set; }
        public string PoNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal Vat { get; set; }
        public string Notes { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}