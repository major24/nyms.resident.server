using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nyms.resident.server.DataProviders.Interfaces
{
    public interface IDatabaseSetupProvider
    {
        void SeedDatabase();
        void ClearDatabase();
    }
}
