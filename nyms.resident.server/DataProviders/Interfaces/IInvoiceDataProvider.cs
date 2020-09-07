using nyms.resident.server.Invoice;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IInvoiceDataProvider
    {
        IEnumerable<Schedule> GetAllSchedules();
        IEnumerable<Schedule> GetAllSchedulesForInvoiceDate(DateTime billingStart, DateTime billingEnd);
    }
}