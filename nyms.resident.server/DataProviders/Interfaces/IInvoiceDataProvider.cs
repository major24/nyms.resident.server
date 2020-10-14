using nyms.resident.server.Invoice;
using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IInvoiceDataProvider
    {
        IEnumerable<SchedulePayment> GetAllSchedulesForInvoiceDate(DateTime billingStart, DateTime billingEnd);
        Task<IEnumerable<BillingCycle>> GetBillingCycles();
        Task<bool> UpdateValidatedInvoices(IEnumerable<InvoiceValidatedEntity> InvoiceValidatedEntities);
        Task<bool> InsertInvoiceComments(InvoiceCommentsEntity invoiceCommentsEntity);
        Task<IEnumerable<InvoiceValidatedEntity>> GetValidatedInvoices(int localAuthorityId, int billingCycleId);
        Task<IEnumerable<InvoiceCommentsEntity>> GetInvoiceComments(int localAuthorityId, int billingCycleId);
    }
}