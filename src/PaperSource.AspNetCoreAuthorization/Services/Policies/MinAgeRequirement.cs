using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreAuthTests.Controllers
{
    public class MinAgeRequirement : IAuthorizationRequirement
    {
        public MinAgeRequirement(int age)
        {
            Age = age;
        }

        public int Age { get; private set; }
    }
}