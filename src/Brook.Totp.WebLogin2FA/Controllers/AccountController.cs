
using Brook.Totp.WebLogin2FA.Cache;
using Brook.Totp.WebLogin2FA.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Brook.Totp.WebLogin2FA.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ITotp _totp;
        private readonly IUserService _userService;
        public AccountController(ITotp totp, IUserService userService)
        {
            _totp = totp;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userService.GetUserAsync(email);
            if (user == null)
            {
                return Json(new { result = 0, msg = "用户不存在" });
            }

            if (user.PassWord != password)
            {
                return Json(new { result = 0, msg = "账号密码错误" });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
            };

            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };


            if (!user.IsOpen2FA)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Member"));
                
                await HttpContext.SignInAsync(
                 CookieAuthenticationDefaults.AuthenticationScheme,
                 new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                 authProperties);

                return Json(new { result = 1, msg = "登录成功", url = "/Account/Bind" });
            };


            claims.Add(new Claim(ClaimTypes.Role, "2FA"));

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)),
                authProperties);

            return Json(new { result = 2, msg = "校验2FA" });
        }

        [Authorize]
        /// <summary>
        /// 绑定页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Bind()
        {
            return View();
        }

        [Authorize]
        public IActionResult GetQr()
        {
            var totpSetup = _totp.GenerateUrl("dotNETBuild", CurremtUser.Email, CurremtUser.SecretKeyFor2FA);

            return Json(new { qrCodeContennt = totpSetup.QrCodeImageContent });
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Valid(int code)
        {
            var valid = _totp.Validate(CurremtUser.SecretKeyFor2FA
                , code, 30);

            if (!valid)
            {
                return Json(new { result = 0, msg = "2FA校验失败" });
            }
            //校验成功后 如果是第一次绑定校验 需将用户的accountSecretKey 存入数据库
            CurremtUser.IsOpen2FA = true;

            await _userService.UpdateAsync(CurremtUser);

            _cacheManage.Remove(string.Format(CacheKeys.GetUserForEmail, CurremtUser.Email));

            var claims = new List<Claim>
            {
                new Claim("user", CurremtUser.Email),
                new Claim("role", "Member")
            };

            await HttpContext.SignInAsync(new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));
            return Json(new { result = 1, msg = "2FA校验成功", url = "/Home/Index" });

        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

    }
}
