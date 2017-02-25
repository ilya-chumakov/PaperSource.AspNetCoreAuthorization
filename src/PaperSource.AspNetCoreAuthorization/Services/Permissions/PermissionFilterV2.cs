using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PaperSource.AspNetCoreAuthorization.Services.Permissions
{
    public class PermissionFilterV2 : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authService;
        private readonly PermissionRequirement _requirement;

        public PermissionFilterV2(IAuthorizationService authService, PermissionRequirement requirement)
        {
            _authService = authService;
            _requirement = requirement;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool ok = await _authService.AuthorizeAsync(context.HttpContext.User, null, _requirement);

            if (!ok) context.Result = new ChallengeResult();
        }
    }
}