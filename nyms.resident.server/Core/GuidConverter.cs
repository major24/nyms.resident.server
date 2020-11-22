using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nyms.resident.server.Core
{
    public static class GuidConverter
    {
        public static Guid Convert(string guid)
        {
            try
            {
                return Guid.ParseExact(guid, "D");
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException("The string to be parsed is null.");
            }
            catch (FormatException)
            {
                throw new ArgumentException($"Bad Format: {guid}");
            }
        }

        public static bool IsValid(string guid)
        {
            var id = Convert(guid);
            return id != Guid.Empty;
        }
    }
}