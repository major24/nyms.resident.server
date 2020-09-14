using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IResidentDataProvider
    {
        IEnumerable<Resident> GetResidentsByCareHomeId(int careHomeId);
        IEnumerable<Resident> GetResidentsForInvoice(DateTime billingStart, DateTime billingEnd);
        bool UpdateExitDate(Guid referenceId, DateTime exitDate);
    }
}
