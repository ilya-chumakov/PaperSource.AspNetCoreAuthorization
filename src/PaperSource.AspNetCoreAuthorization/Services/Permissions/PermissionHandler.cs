using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PaperSource.AspNetCoreAuthorization.Services.Permissions
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (requirement.Permissions.Any()) //TODO: your code
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}