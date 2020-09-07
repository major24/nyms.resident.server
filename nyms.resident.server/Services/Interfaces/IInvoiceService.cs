using nyms.resident.server.Invoice;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IInvoiceService
    {
        IEnumerable<InvoiceResident> GetAllSchedules(DateTime billingBeginDate, DateTime billingEndDate);
    }
}
