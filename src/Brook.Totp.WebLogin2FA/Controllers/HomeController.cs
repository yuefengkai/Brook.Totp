using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Brook.Totp;
using Brook.Totp.WebLogin2FA.Models;
using Brook.Totp.WebLogin2FA.Cache;
using Microsoft.AspNetCore.Authorization;

namespace Brook.Totp.WebLogin2FA
{
    //[Authorize]
    public class HomeController : BaseController
    {
        private readonly ITotp _totp;

        public HomeController(ITotp totp)
        {
            _totp = totp;
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.CurrentUser = CurremtUser;
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
