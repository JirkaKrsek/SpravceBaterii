using Microsoft.EntityFrameworkCore;
using SpravceBaterii.Components;
using SpravceBaterii.Data;

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
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
