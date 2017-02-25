using System;
using Microsoft.AspNetCore.Mvc;

namespace PaperSource.AspNetCoreAuthorization.Services.Permissions
{
    public class AuthorizePermissionAttribute : TypeFilterAttribute
    {
        public AuthorizePermissionAttribute(params Permission[] permissions)
            : base(typeof(PermissionFilterV2))
        {
            Arguments = new[] { new PermissionRequirement(permissions) };
            Order = Int32.MinValue;
        }
    }
}