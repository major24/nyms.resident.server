using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IResidentDataProvider
    {
        IEnumerable<Resident> GetResidents();
        Resident GetResident(Guid referenceId);
        Task<ResidentEntity> Create(ResidentEntity residentEntity);
        Task<ResidentEntity> Update(ResidentEntity residentEntity);
        bool DischargeResident(Guid referenceId, DateTime dischargeDate);
        bool ExitInvoiceResident(Guid referenceId, DateTime exitDate);
        bool ActivateResident(Guid referenceId);
    }
}
