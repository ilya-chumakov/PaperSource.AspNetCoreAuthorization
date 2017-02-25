using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperSource.AspNetCoreAuthorization.Services.Permissions;

namespace PaperSource.AspNetCoreAuthorization.Controllers
{
    public class PermissionController : Controller
    {
        [TypeFilter(typeof(PermissionFilterV1),
            Arguments = new object[] { new[] { Permission.Foo, Permission.Bar } })]
        public IActionResult ExampleV1()
        {
            return View("OK");
        }

        [AuthorizePermission(Permission.Foo, Permission.Bar)]
        [Authorize(Policy = "age-adult-policy")]
        public IActionResult ExampleV2()
        {
            return View("OK");
        }
    }
}