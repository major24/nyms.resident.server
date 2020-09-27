﻿using nyms.resident.server.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IEnquiryDataProvider
    {
        IEnumerable<Enquiry> GetAll();
        Task<EnquiryEntity> GetByReferenceId(Guid referenceId);
        Task<Enquiry> Create(Enquiry enquiry);
    }
}