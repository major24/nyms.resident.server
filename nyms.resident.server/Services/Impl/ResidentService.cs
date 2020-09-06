using nyms.resident.server.DataProviders.Interfaces;
using nyms.resident.server.Models;
using nyms.resident.server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Services.Impl
{
    public class ResidentService : IResidentService
    {
        private readonly IResidentDataProvider _residentDataProvider;
        public ResidentService(IResidentDataProvider residentDataProvider)
        {
            _residentDataProvider = residentDataProvider ?? throw new ArgumentException(nameof(residentDataProvider));
        }

        public IEnumerable<Resident> GetAll()
        {
            return this._residentDataProvider.GetAll();
        }
    }
}