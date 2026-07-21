using Microsoft.AspNetCore.Authorization;

namespace Online_Whiteboard_Backend
{
    public class OptionalAuthenticationHandler : AuthorizationHandler<OptionalAuthenticationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OptionalAuthenticationRequirement requirement)
        {
            Thread.CurrentPrincipal = context.User;
            //if (context.User != null)
            //{
            //    context.User.
            //}
            //context.User.Identity.

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
