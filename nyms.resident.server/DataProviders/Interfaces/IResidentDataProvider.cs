using nyms.resident.server.Invoice;
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
        Resident GetResident(Guid referenceId);
        IEnumerable<Resident> GetResidentsForInvoice(DateTime billingStart, DateTime billingEnd);
        bool UpdateExitDate(Guid referenceId, DateTime exitDate);
        Task<ResidentEntity> Create(ResidentEntity resident);
        Task<ResidentEntity> Update(ResidentEntity resident);
    }
}
