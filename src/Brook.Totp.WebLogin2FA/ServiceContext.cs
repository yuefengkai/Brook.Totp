using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brook.Totp.WebLogin2FA
{
    public static class ServiceContext
    {
        public static IServiceProvider Instance { get; set; }
    }
}
