using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using PaperSource.AspNetCoreAuthorization.Models;

namespace AspNetCoreAuthTests.Controllers
{
    public class ResourceHandlerV2 : AuthorizationHandler<OperationAuthorizationRequirement, Order>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            Order order)
        {
            // Validate the requirement against the resource and identity.
            if (context.User.Identity.IsAuthenticated) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}