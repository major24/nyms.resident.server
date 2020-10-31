using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Services.Core
{
    public static class Nlogger2
    {
        public static Logger GetLogger()
        {
            var logger = LogManager.GetCurrentClassLogger();
            var d1 = (DatabaseTarget)logger.Factory.Configuration
                .AllTargets.Where(t => t.Name == "database").FirstOrDefault();
            d1.ConnectionString = Util.DecryptString(ConfigurationManager.AppSettings["connectionString_nyms24"]);
            return logger;
        }
    }
}