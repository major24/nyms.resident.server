using nyms.resident.server.Model;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class BudgetListResponse
    {
        public int Id { get; set; }
        public Guid ReferenceId { get; set; }
        public int SpendCategoryId { get; set; }
        public int CareHomeId { get; set; }
        public string Name { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string PoPrefix { get; set; }
        public Decimal BudgetTotal { get; set; }
        public string Approved { get; set; }
        public Decimal SpendTotal { get; set; }
        public Decimal VatTotal { get; set; }
        public string CareHomeName { get; set; }
        public string SpendCategoryName { get; set; }

        public IEnumerable<SpendResponse> SpendResponses { get; set; }
    }
}