using System.Security.Claims;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;


namespace Gql.EmptyExample.Interceptor
{
    public class MinimumAgeRequirement 
    {
        public MinimumAgeRequirement(int minimumAge) =>
            MinimumAge = minimumAge;

        public int MinimumAge { get; }
    }

    //public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement, IResolverContext>
    //{
    //    protected override Task HandleRequirementAsync(
    //        AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    //    {
    //        var dateOfBirthClaim = context.User.FindFirst(
    //            c => c.Type == ClaimTypes.DateOfBirth && c.Issuer == "http://contoso.com");

    //        if (dateOfBirthClaim is null)
    //        {
    //            return Task.CompletedTask;
    //        }

    //        var dateOfBirth = Convert.ToDateTime(dateOfBirthClaim.Value);
    //        int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
    //        if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
    //        {
    //            calculatedAge--;
    //        }

    //        if (calculatedAge >= requirement.MinimumAge)
    //        {
    //            context.Succeed(requirement);
    //        }

    //        return Task.CompletedTask;
    //    }
    //}

}
