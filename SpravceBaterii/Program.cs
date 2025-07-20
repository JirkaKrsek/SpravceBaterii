using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SpravceBaterii;
using SpravceBaterii.Components;
using SpravceBaterii.Data;
using SpravceBaterii.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


// Název systémové promìnné pro pøístup k databázi
string systemVariableName = "SpravceBateriiDatabaze";

// Naètení connection stringu ze systémové promìnné
var connectionString = Environment.GetEnvironmentVariable(systemVariableName);

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("The environment variable " + systemVariableName + " is missing or empty.");
}

// Registrace DbContext s connection stringem + automatická obnova pøipojení k databázi v pøípadì výpadku
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, sql => sql.EnableRetryOnFailure())
);

// Nastavení Identity uživatele
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    // Pøihlášení bez nutnosti ovìøeného emailu
    options.SignIn.RequireConfirmedAccount = false;

    // Každý email musí být v databázi jedineèný
    options.User.RequireUniqueEmail = true;

    // Nastavení požadavkù hesla
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager()
    .AddErrorDescriber<CzechIdentityErrorDescriber>(); // Pøeklad chybových hlášek do èeštiny

// Pøidání autentizace s cookie
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
})
    .AddCookie(IdentityConstants.ApplicationScheme);

// Pøidání autorizace
builder.Services.AddAuthorization();

// Úprava nastavení cookie
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.SlidingExpiration = true;
    options.LoginPath = "/ucet/prihlaseni";
    options.LogoutPath = "/ucet/odhlaseni";
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// Pøidání autentizace a autorizace
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
