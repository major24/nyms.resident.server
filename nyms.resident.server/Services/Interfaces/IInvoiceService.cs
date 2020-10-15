using nyms.resident.server.Invoice;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IInvoiceService
    {
        InvoiceData GetInvoiceData(DateTime startDate, DateTime endDate);
        InvoiceData GetInvoiceData(int localAuthorityId, int billingCycleId);
        Task<IEnumerable<BillingCycle>> GetBillingCycles();
        Task<bool> UpdateInvoicesValidated(InvoiceValidatedEntity[] invoiceValidatedEntities);
        Task<bool> InsertInvoiceComments(InvoiceCommentsEntity invoiceCommentsEntity);
        // Task<IEnumerable<InvoiceCommentsEntity>> GetInvoiceComments(int localAuthorityId, int billingCycleId);
    }
}
