using Brook.Totp.WebLogin2FA.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brook.Totp.WebLogin2FA.Services;

namespace Brook.Totp.WebLogin2FA
{
    public class BaseController : Controller
    {
        public readonly ICacheManage _cacheManage;
        public BaseController()
        {
            _cacheManage = ServiceContext.Instance.GetService<ICacheManage>();
        }

        public Models.User CurremtUser {
            get
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return null;
                }

                var _userService1 = this.HttpContext.RequestServices.GetService<IUserService>();

                var user = _cacheManage.GetOrCreate<Models.User>(string.Format(CacheKeys.GetUserForEmail, HttpContext.User.Identity.Name), entry =>
                {
                    return _userService1.GetUserAsync(HttpContext.User.Identity.Name).GetAwaiter().GetResult();
                });

                return user;
            }
        }
    }
}
