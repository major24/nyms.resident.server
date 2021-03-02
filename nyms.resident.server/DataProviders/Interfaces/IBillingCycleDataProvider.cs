using nyms.resident.server.Invoice;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IBillingCycleDataProvider
    {
        Task<IEnumerable<BillingCycle>> GetBillingCycles();
    }
}
