using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaperSource.AspNetCoreAuthorization.Models;

namespace PaperSource.AspNetCoreAuthorization.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HomeController>();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm, string returnUrl = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Webpage, "http://goo.gl", ClaimValueTypes.String),
                new Claim(ClaimTypes.Name, "Fake User"),
                new Claim("age", "25", ClaimValueTypes.Integer),
                new Claim("permission-foo", "grant")
            };

            var identity = new ClaimsIdentity("MyCookieMiddlewareInstance");
            identity.AddClaims(claims);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.Authentication.SignInAsync("MyCookieMiddlewareInstance",
                principal,
                new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                });

            _logger.LogInformation(4, "User logged in.");

            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.Authentication.SignOutAsync("MyCookieMiddlewareInstance");

            _logger.LogInformation(4, "User logged out.");

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
