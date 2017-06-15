using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperSource.AspNetCoreAuthorization.Models;
using PaperSource.AspNetCoreAuthorization.Services.Resources;

namespace PaperSource.AspNetCoreAuthorization.Controllers
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
            Order order = new Order(); //get resourse from DB

            if (await _authorizationService.AuthorizeAsync(this.User, order, "resource-allow-policy"))
            {
                return View("OK");
            }

            return new ChallengeResult(); //it produces 401 or 403 response (depending on user state)
        }

        public async Task<IActionResult> ExampleV2()
        {
            var requirements = new [] { Operations.Create, Operations.Read, Operations.Update };

            if (await _authorizationService.AuthorizeAsync(this.User, new Order(), requirements))
            {
                return View("OK");
            }

            return new ChallengeResult();  //it produces 401 or 403 response (depending on user state)
        }
    }
}
