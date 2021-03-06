﻿using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nyms.resident.server.Services.Interfaces
{
    public interface IEnquiryService
    {
        IEnumerable<Enquiry> GetAll();
        Task<Enquiry> GetByReferenceId(Guid referenceId);
        Task<Enquiry> Create(Enquiry enquiry);
        Task<Enquiry> Update(Enquiry enquiry);
        IEnumerable<EnquiryAction> GetActions(Guid referenceId);
        void SaveActions(Guid referenceId, EnquiryAction[] enquiryActions);
    }
}
