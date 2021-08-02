using nyms.resident.server.Model;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class BudgetListResponse : Budget
    {
        public Decimal BudgetTotal { get; set; }
        public string Approved { get; set; }
        public Decimal SpendTotal { get; set; }
        public string CareHomeName { get; set; }
        public string SpendCategoryName { get; set; }

        public IEnumerable<BudgetAllocation> BudgetAllocations { get; set; }
        public IEnumerable<Spend> Spends { get; set; }
    }
}