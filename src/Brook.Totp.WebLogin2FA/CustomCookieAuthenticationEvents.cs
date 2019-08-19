using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brook.Totp.WebLogin2FA
{
    public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
    {
        //private readonly IUserRepository _userRepository;
        public CustomCookieAuthenticationEvents()
        {
            // Get the database from registered DI services.
            //_userRepository = userRepository;
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var userPrincipal = context.Principal;

            var role2Fa = (from c in userPrincipal.Claims
                               where c.Type == ClaimTypes.Role
                               select c.Value).FirstOrDefault();

           var allow2FAAction = "account/valid";

            if (role2Fa == "2FA")//2fa只能处理2fa校验
            {
                if (context.Request.Path.ToString().ToLower().Contains(allow2FAAction))
                {
                    return;
                }
                else
                {
                    context.RejectPrincipal();

                    await context.HttpContext.SignOutAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
        }
    }
}
