using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Webpage, "http://goo.gl", ClaimValueTypes.String),
                new Claim("age", "25", ClaimValueTypes.String),
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

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.Authentication.SignOutAsync("MyCookieMiddlewareInstance");

            _logger.LogInformation(4, "User logged out.");

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
