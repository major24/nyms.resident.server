using nyms.resident.server.Invoice;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IInvoiceDataProvider
    {
        IEnumerable<Schedule> GetAllSchedules();
    }
}