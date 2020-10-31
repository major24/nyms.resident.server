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
        IEnumerable<Resident> GetResidentsByCareHomeId(int careHomeId);
        Resident GetResident(Guid referenceId);
        bool UpdateExitDate(Guid referenceId, DateTime exitDate);
        Task<Resident> Create(Resident resident);
        Task<Resident> Create(Enquiry enquiry);
        Task<Resident> Update(Resident resident);
    }
}
