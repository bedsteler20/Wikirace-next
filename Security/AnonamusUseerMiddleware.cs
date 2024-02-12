
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Wikirace.Data;

namespace Wikirace.Security;

/// <summary>
/// If a user connects to the server and is not signed in than we
/// will create an anonymous user for them. This user will be
/// used to track their progress in the game.
/// </summary>
public class AnonymousUserMiddleware : IMiddleware {
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        if (context.User?.Identity?.IsAuthenticated ?? false) {
            await next(context);
            return;
        }

        var userManager = context.RequestServices.GetRequiredService<UserManager<AppUser>>();
        var signInManager = context.RequestServices.GetRequiredService<SignInManager<AppUser>>();

        var id = Guid.NewGuid().ToString();
        var user = new AppUser {
            Id = id,
            UserName = "Anonymous-" + id,
            IsAnonymous = true

        };

        var result = await userManager.CreateAsync(user);

        if (result.Succeeded) {
            await signInManager.SignInAsync(user, isPersistent: true);
        }

        _ = context.User!.Claims.Append(new Claim(ClaimTypes.Anonymous, "true"));

        await next(context);
    }
}

public static class AnonymousUserMiddlewareExtensions {
    public static IApplicationBuilder UseAnonymousUser(this IApplicationBuilder builder) {
        return builder.UseMiddleware<AnonymousUserMiddleware>();
    }

    public static IServiceCollection AddAnonymousUserMiddleware(this IServiceCollection services) {
        return services.AddTransient<AnonymousUserMiddleware>();
    }
}