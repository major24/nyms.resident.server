using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class SpendBudgetRequest : SpendBudget
    {
        public IEnumerable<SpendBudgetAllocation> SpendBudgetAllocations { get; set; }
        // public SpendBudgetAllocation SpendBudgetAllocation { get; set; }
    }
}