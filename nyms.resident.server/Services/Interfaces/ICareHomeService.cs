using nyms.resident.server.Models;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.Services.Interfaces
{
    public interface ICareHomeService
    {
        IEnumerable<CareHome> GetCareHomes();
        IEnumerable<CareHome> GetCareHomesDetails();
        CareHome GetCareHomesDetails(int id);
    }
}
