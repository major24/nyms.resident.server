using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace nyms.resident.server.Services.Core
{
    public static class Util
    {
        public static string DecryptString(string encrString)
        {
            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(encrString);
                decrypted = ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                throw new Exception("Error in decrypting dbstring");
            }
            return decrypted;
        }
    }
}