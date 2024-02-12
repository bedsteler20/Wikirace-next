using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wikirace.Security;
using Wikirace.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddMvcCore(options => {
    options.EnableEndpointRouting = false;
});
// Database stuff
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=wikirace.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Email

// builder.Services.AddTransient<IEmailSender, EmailSenderService>();
// builder.Services.Configure<EmailSenderOptions>(builder.Configuration);


// Accounts
var identity = builder.Services.AddDefaultIdentity<AppUser>(options => {
});
identity.AddEntityFrameworkStores<AppDbContext>();
identity.AddDefaultUI();

builder.Services.AddSession();
builder.Services.AddSingleton<IAuthorizationHandler, IsInGameRequirementHandler>();
builder.Services.AddAuthorization(options => {
    options.AddPolicy("IsInGame", policy =>
        policy.Requirements.Add(new IsInGameRequirement()));
});
// Routing
builder.Services.AddRouting(options => {
    options.LowercaseUrls = true;
});

// Game state
builder.Services.AddServerSentEvents();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthentication();
app.MapRazorPages();

app.MapServerSentEvents("/game/{gameId}/state");
app.UseMvc();

// app.MapAreaControllerRoute(
//     name: "Game.JoinGame",
//     areaName: "Game",
//     pattern: "join",
//     defaults: new { controller = "JoinGame", action = "Index" }
// );

// app.MapAreaControllerRoute(
//     name: "Game.CreateGame",
//     areaName: "Game",
//     pattern: "create",
//     defaults: new { controller = "CreateGame", action = "Index" }
// );

// app.MapAreaControllerRoute(
//     name: "Game.Lobby",
//     areaName: "Game",
//     pattern: "game/{gameId}/{controller=Lobby}/{action=Index}"
// );

// app.MapAreaControllerRoute(
//     name: "Game.Lobby.ListPlayers",
//     areaName: "Game",
//     pattern: "game/{gameId}/{controller=Lobby}/{action=ListPlayers}"
// );

// app.MapAreaControllerRoute(
//     name: "Game.Play",
//     areaName: "Game",
//     pattern: "game/{gameId}/play",
//     defaults: new { controller = "Play", action = "Index" }
// );

// app.MapAreaControllerRoute(
//     name: "Game.Manage.EndGame",
//     areaName: "Game",
//     pattern: "game/{gameId}/manage/end-game",
//     defaults: new { controller = "Manage", action = "EndGame" }
// );

// app.MapAreaControllerRoute(
//     name: "Game.Manage.LeaveGame",
//     areaName: "Game",
//     pattern: "game/{gameId}/manage/leave-game",
//     defaults: new { controller = "Manage", action = "LeaveGame" }
// );

// app.MapAreaControllerRoute(
//     name: "Game.Manage.StartGame",
//     areaName: "Game",
//     pattern: "game/{gameId}/manage/start-game",
//     defaults: new { controller = "Manage", action = "StartGame" }
// );

// app.MapAreaControllerRoute(
//     name: "Game.Manage.KickPlayer",
//     areaName: "Game",
//     pattern: "game/{gameId}/manage/kick-player",
//     defaults: new { controller = "Manage", action = "KickPlayer" }
// );


// app.MapAreaControllerRoute(
//     name: "Game.PageFrame",
//     areaName: "Game",
//     pattern: "game/{gameId}/page-frame",
//     defaults: new { controller = "PageFrame", action = "Index" });

// app.MapControllerRoute(
//     name: "Default",
//     pattern: "{controller=Home}/{action=Index}"
// );
app.Run();
