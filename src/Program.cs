using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Wikirace.Security;
using Wikirace.Data;
using Wikirace.Repository;
using Wikirace.Services;
using Wikirace.Utils;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddMvcCore(options => {
    options.EnableEndpointRouting = false;
});

// Database stuff

builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDatabaseRepository();

builder.Services.Configure<DbCleanupOptions>(builder.Configuration);
builder.Services.AddDbCleanupService();


// Email
builder.Services.AddTransient<IEmailSender, EmailSenderService>();
builder.Services.Configure<EmailSenderOptions>(options => {
    options.SendGridKey = builder.Configuration["SendGridKey"] ?? Environment.GetEnvironmentVariable("SEND_GRID_KEY");

});

// Accounts
var identity = builder.Services.AddDefaultIdentity<AppUser>(options => {
});
identity.AddEntityFrameworkStores<AppDbContext>();
identity.AddDefaultUI();

builder.Services.AddSession();

builder.Services.AddSingleton<IAuthorizationHandler, IsInGameRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, IsGameOwnerRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, NotAnonymousRequirementHandler>();

builder.Services.AddAuthorization(options => {
    options.AddPolicy(Polices.IsInGame, policy =>
        policy.Requirements.Add(new IsInGameRequirement()));
    options.AddPolicy(Polices.IsGameOwner, policy =>
        policy.Requirements.Add(new IsGameOwnerRequirement()));
    options.AddPolicy(Polices.NotAnonymous, policy =>
        policy.Requirements.Add(new NotAnonymousRequirement()));
});
builder.Services.AddAnonymousUserMiddleware();

// Routing
builder.Services.AddRouting(options => {
    options.LowercaseUrls = true;
});

// Game state
builder.Services.AddServerSentEvents();
builder.Services.AddSingleton<ClientEventsService>();

builder.Services.AddWikipediaRepo();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope()) {
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
}



app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthentication();
app.UseAnonymousUser();
app.MapRazorPages();
app.MapServerSentEvents("/game/{gameId}/state");
app.UseMvc();

app.Run();