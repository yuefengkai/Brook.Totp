using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brook.Totp.WebLogin2FA.Models
{

    public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public bool IsOpen2FA { get; set; }
        public string SecretKeyFor2FA { get; set; }
    }
}
