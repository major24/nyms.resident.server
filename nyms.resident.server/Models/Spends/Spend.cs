using nyms.resident.server.Models;
using System;

namespace nyms.resident.server.Model
{
    public class Spend
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public string PoNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal Vat { get; set; }
        public string TranType { get; set; }
        public SpendComments[] SpendComments { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedByName { get; set; }
    }
}