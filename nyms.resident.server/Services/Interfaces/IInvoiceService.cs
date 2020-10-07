using nyms.resident.server.Invoice;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IInvoiceService
    {
        IEnumerable<InvoiceResident> GetInvoiceData(DateTime billingBeginDate, DateTime billingEndDate);
        IEnumerable<InvoiceResident> GetInvoiceData(int localAuthorityId, int billingCycleId);
        Task<IEnumerable<BillingCycle>> GetBillingCycles();
    }
}
