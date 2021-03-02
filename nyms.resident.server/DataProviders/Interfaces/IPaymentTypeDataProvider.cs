using nyms.resident.server.Models;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IPaymentTypeDataProvider
    {
        IEnumerable<PaymentType> GetPaymentTypes();
    }
}
