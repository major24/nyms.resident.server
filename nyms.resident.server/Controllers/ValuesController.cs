using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace nyms.resident.server.Controllers
{
    public class ValuesController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: api/Values
        public IEnumerable<string> Get()
        {
            string name = "Major";
            string v = "Nalliah";
            logger.Info("You have visited the values " + Environment.NewLine + DateTime.Now);
            logger.Info($"Person visited is {name} - {v} at {DateTime.UtcNow}");

            return new string[] { "value1", "value2" };
        }

        // GET: api/Values/5
        public string Get(int id)
        {
            logger.Info($"You have visited the values with id {id}");
            try
            {
                int x = 5 / id;
            }
            catch(Exception ex)
            {
                logger.Error(ex, "Got exception");
            }
            return "value";
        }

        // POST: api/Values
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Values/5
        public void Delete(int id)
        {
        }
    }
}
