using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Wikirace.Data;
using Wikirace.Utils;

namespace Wikirace.Security;



public class NotAnonymousRequirement : IAuthorizationRequirement {}


public class NotAnonymousRequirementHandler : AuthorizationHandler<NotAnonymousRequirement> {

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, NotAnonymousRequirement requirement) {
        var httpContext = context.Resource?.PullProperty<HttpContext>("HttpContext");
        var userManager = httpContext?.RequestServices.GetRequiredService<UserManager<AppUser>>();

        if (userManager is null) {
            context.Fail();
            return;
        }

        var user = await userManager.GetUserAsync(httpContext!.User);
        
        if (user is null || user.IsAnonymous) {
            context.Fail();
            return;
        }
        
        context.Succeed(requirement);
    }
}