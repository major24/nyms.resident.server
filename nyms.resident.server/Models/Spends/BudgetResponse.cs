using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class BudgetResponse : Budget
    {
        public IEnumerable<BudgetAllocation> BudgetAllocations { get; set; }
    }
}