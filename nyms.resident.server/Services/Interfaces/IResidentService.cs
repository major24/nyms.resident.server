using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IResidentService
    {
        IEnumerable<Resident> GetAllResidentsByCareHomeId(int careHomeId);
        IEnumerable<Resident> GetActiveResidentsByCareHomeId(int careHomeId);
        Resident GetResident(Guid referenceId);
        bool DischargeResident(Guid referenceId, DateTime exitDate);
        bool ActivateResident(Guid referenceId);
        Task<Resident> Update(Resident resident);
        Task<Resident> AdmitEnquiry(ResidentRequest resident);
    }
}
