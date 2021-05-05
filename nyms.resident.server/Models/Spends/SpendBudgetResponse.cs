using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class SpendBudgetResponse : SpendBudget
    {
        public IEnumerable<SpendBudgetAllocation> SpendBudgetAllocations { get; set; }
    }
}