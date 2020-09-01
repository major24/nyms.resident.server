using nyms.resident.server.Models;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface ICareHomeDataProvider
    {
        IEnumerable<CareHome> GetCareHomesDetails();
        CareHome GetCareHomeByReferenceId(Guid referenceId);
        IEnumerable<LocalAuthority> GetLocalAuthorities();
    }
}
