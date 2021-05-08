using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class BudgetRequest : Budget
    {
        public IEnumerable<BudgetAllocation> BudgetAllocations { get; set; }
    }
}