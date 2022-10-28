using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenantAPI.Helper
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string mesage)
        {
            response.Headers.Add("Application-Error", mesage);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static bool HasDigits(this string testString)
        {
            foreach (char elem in testString.ToCharArray())
            {
                if (Char.IsDigit(elem)) return true;
            }

            return false;
        }
    }
}
