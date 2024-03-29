﻿using Microsoft.AspNetCore.Authorization;
using Wikirace.Data;
using Wikirace.Security;
using Wikirace.Utils;

namespace Wikirace.Data;

public class IsGameOwnerRequirement : IAuthorizationRequirement { }


/// <summary>
/// Handles the authorization requirement for determining if the current user is the owner of the game.
/// </summary>
public class IsGameOwnerRequirementHandler : AuthorizationHandler<IsGameOwnerRequirement> {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsGameOwnerRequirement requirement) {
        var isInGameReq = context.Requirements.IndexOfType<IsInGameRequirement>();
        var ownerReq = context.Requirements.IndexOfType<IsGameOwnerRequirement>();


        if (isInGameReq is null || isInGameReq > ownerReq) {
            throw new InvalidOperationException("IsGameOwnerRequirement must be used after IsInGameRequirement.");
        }

        var httpContext = context.Resource?.PullProperty<HttpContext>("HttpContext");
        var game = httpContext?.Items["Game"] as Game;
        var player = httpContext?.Items["Player"] as Player;

        if (game is null || player is null || !player.IsOwner) {
            context.Fail();
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}