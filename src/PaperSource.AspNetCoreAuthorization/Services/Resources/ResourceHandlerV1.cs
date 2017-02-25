using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PaperSource.AspNetCoreAuthorization.Models;

namespace PaperSource.AspNetCoreAuthorization.Services.Resources
{
    public class ResourceHandlerV1 : AuthorizationHandler<ResourceBasedRequirement, Order>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ResourceBasedRequirement requirement,
            Order order)
        {
            // TODO: Validate the requirement against the resource and identity.
            if (context.User.Identity.IsAuthenticated) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}