using System.Collections.Generic;

namespace nyms.resident.server.Models
{
    public class BudgetRequest : Budget
    {
        public IEnumerable<BudgetAllocation> BudgetAllocations { get; set; }
        public int BudgetMonth { get; set; }
        public int NumberOfMonths { get; set; }
    }
}