using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Wikirace.Data;
using Wikirace.Utils;

namespace Wikirace.Security;

public class IsInGameRequirement : IAuthorizationRequirement { }


public class IsInGameRequirementHandler : AuthorizationHandler<IsInGameRequirement> {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsInGameRequirement requirement) {
        // context.Resource has a dynamic type, when inspecting it in the debugger, it's a internal
        // class so we can't see the properties. We can use the PullProperty extension method to
        // get the properties we need.
        var httpContext = context.Resource?.PullProperty<HttpContext>("HttpContext");
        var actionDescriptor = context.Resource?.PullProperty<ActionDescriptor>("ActionDescriptor");
        var database = httpContext?.GetService<AppDbContext>();
        var gameId = httpContext?.GetRouteValue("gameId")?.ToString();
        var userId = httpContext?.User.GetUserId();

        var game = database?.Games.FirstOrDefault(g => g.Id == gameId);
        var player = game?.Players.FirstOrDefault(p => p.UserId == userId);

        if (game is null || player is null) {
            context.Fail();
            return Task.CompletedTask;
        }

        // Add the game and player to the context so we can access them in the controller.
        httpContext!.Items["Game"] = game;
        httpContext!.Items["Player"] = player;

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}