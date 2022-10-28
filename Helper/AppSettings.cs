using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TenantAPI.Helper
{
    public class AppSettings
    {
        public string Token { get; set; }
        public string AngularRoot { get; set; }
        public string AccessFailedCount { get; set; }
        public int LockoutHours { get; set; }
    }
}
