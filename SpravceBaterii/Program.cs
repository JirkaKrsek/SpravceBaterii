using Microsoft.EntityFrameworkCore;
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

// Registrace DbContext s connection stringem
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Nastavení Identity uživatele
builder.Services.AddDefaultIdentity<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


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
