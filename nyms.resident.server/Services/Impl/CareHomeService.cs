using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace nyms.resident.server.Services.Impl
{
    public class CareHomeService : ICareHomeService
    {
        private readonly ICareHomeDataProvider _careHomeDataProvider;
        public CareHomeService(ICareHomeDataProvider careHomeDataProvider)
        {
            _careHomeDataProvider = careHomeDataProvider ?? throw new ArgumentException(nameof(careHomeDataProvider));
        }

        public IEnumerable<CareHome> GetCareHomes()
        {
            return _careHomeDataProvider.GetCareHomes();
        }

        public IEnumerable<CareHome> GetCareHomesDetails()
        {
            return _careHomeDataProvider.GetCareHomesDetails();
        }

        public CareHome GetCareHomesDetails(int id)
        {
            var careHomeDetails = this.GetCareHomesDetails();
            return careHomeDetails.Where(ch => ch.Id == id).FirstOrDefault();
        }
    }
}