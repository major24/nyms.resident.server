using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IResidentDataProvider
    {
        IEnumerable<Resident> GetAllResidentsByCareHomeId(int careHomeId);
        IEnumerable<Resident> GetActiveResidentsByCareHomeId(int careHomeId);
        Resident GetResident(Guid referenceId);
        IEnumerable<Resident> GetResidentsForInvoice(DateTime billingStart, DateTime billingEnd);
        bool DischargeResident(Guid referenceId, DateTime exitDate);
        bool ActivateResident(Guid referenceId);
        Task<ResidentEntity> Create(ResidentEntity residentEntity);
        Task<ResidentEntity> Update(ResidentEntity residentEntity);
        IEnumerable<ResidentContact> GetResidentContactsByResidentId(int residentId);
        SocialWorker GetSocialWorker(int residentId);
    }
}
