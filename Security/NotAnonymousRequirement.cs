using Microsoft.AspNetCore.Authorization;
using Wikirace.Utils;

namespace Wikirace;



public class NotAnonymousRequirement : IAuthorizationRequirement {}


public class NotAnonymousRequirementHandler : AuthorizationHandler<NotAnonymousRequirement> {

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NotAnonymousRequirement requirement) {
        if (context.User.IsAnonymous()) {
            context.Fail();
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}