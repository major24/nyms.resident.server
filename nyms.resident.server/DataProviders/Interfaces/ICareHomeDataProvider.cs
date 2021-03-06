﻿using nyms.resident.server.Models;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface ICareHomeDataProvider
    {
        IEnumerable<CareHome> GetCareHomes();
        IEnumerable<CareHomeDetail> GetCareHomesDetails();
        CareHomeDetail GetCareHomeByReferenceId(Guid referenceId);
        IEnumerable<LocalAuthority> GetLocalAuthorities();
    }
}
