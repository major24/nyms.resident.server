using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace nyms.resident.server.Services.Impl
{
    public class CareHomeService : ICareHomeService
    {
        private readonly ICareHomeDataProvider _careHomeDataProvider;
        public CareHomeService(ICareHomeDataProvider careHomeDataProvider)
        {
            _careHomeDataProvider = careHomeDataProvider ?? throw new ArgumentException(nameof(careHomeDataProvider));
        }

        public CareHome GetCareHomeByReferenceId(Guid referenceId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CareHome> GetCareHomesDetails()
        {
            try
            {
                return _careHomeDataProvider.GetCareHomesDetails();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting care homes with room details. " + ex.Message);
            }

        }
    }
}