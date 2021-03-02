using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IEnquiryDataProvider
    {
        IEnumerable<Enquiry> GetEnquires();
        Task<EnquiryEntity> GetEnquiry(Guid referenceId);
        Task<int> Create(Enquiry enquiry);
        Task<Enquiry> Update(Enquiry enquiry);
        IEnumerable<EnquiryAction> GetActions(int enquiryId);
        void SaveActions(int enquiryId, EnquiryAction[] enquiryActions);
    }
}
