using System;

namespace nyms.resident.server.Model
{
    public class SpendResponse
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public string PoNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal Vat { get; set; }
        public string Notes { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}