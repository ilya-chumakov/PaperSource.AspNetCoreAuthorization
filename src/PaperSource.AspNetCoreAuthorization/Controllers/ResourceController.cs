using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperSource.AspNetCoreAuthorization.Models;

namespace AspNetCoreAuthTests.Controllers
{
    public class ResourceController : Controller
    {
        IAuthorizationService _authorizationService;

        public ResourceController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> ExampleV1(int id)
        {
            Order order = new Order(); //получим ресурс из БД

            if (await _authorizationService.AuthorizeAsync(this.User, order, "resource-allow-policy"))
            {
                return View("OK");
            }

            return new ChallengeResult(); //вернем 401 или 403 (в зависимости от состояния пользователя)
        }

        public async Task<IActionResult> ExampleV2()
        {
            var requirements = new [] { Operations.Create, Operations.Read, Operations.Update };

            if (await _authorizationService.AuthorizeAsync(this.User, new Order(), requirements))
            {
                return View("OK");
            }

            return new ChallengeResult(); //вернем 401 или 403 (в зависимости от состояния пользователя)
        }
    }
}
