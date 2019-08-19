using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brook.Totp.WebLogin2FA
{
    public class BrookTotpDBContext: DbContext
    {
        public BrookTotpDBContext(DbContextOptions<BrookTotpDBContext> options)
        : base(options)
        {
        }

        public DbSet<Models.User> Users { get; set; }
    }
}
