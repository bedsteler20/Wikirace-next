using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Wikirace.Security;
using Wikirace.Data;
using Wikirace.Repository;
using Wikirace.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddMvcCore(options => {
    options.EnableEndpointRouting = false;
});

// Database stuff
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=wikirace.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDatabaseRepository();

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
    options.AddPolicy("IsGameOwner", policy =>
        policy.Requirements.Add(new IsGameOwnerRequirement()));
});

// Routing
builder.Services.AddRouting(options => {
    options.LowercaseUrls = true;
});

// Game state
builder.Services.AddServerSentEvents();
builder.Services.AddSingleton<ClientEventsService>();

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

app.Run();
