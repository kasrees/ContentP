using Microsoft.EntityFrameworkCore;
using KeywordsDashboard;
using Microsoft.OpenApi.Models;
using Infrastructure.Foundation;
using NLog;
using Infrastructure.Logging;
using Infrastructure.Middleware;
using Application.Models;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.AddSingleton<ILog, NLogger>();
builder.Services.Configure<UserAuthSettings>(builder.Configuration.GetSection(nameof(UserAuthSettings)));
builder.Services.AddControllersWithViews();

builder.Services.AddDependencies();

builder.Services.AddDbContext<ExtranetPageKeywordsDbContext>(optinos =>
    optinos.UseSqlite(builder.Configuration.GetConnectionString("ExtranetPagesDbConnection"), x => x.MigrationsAssembly("KeywordsDashboard")));

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => 
                {
                    options.LoginPath = "/login";
                    options.Cookie.Name = "extranet_search_dashboard_cookie";
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    };
                });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Page Translations API",
        Description = "An ASP.NET Core Web API for managing Page translations",
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Page Translations API"));
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.UseMiddleware<ExceptionMiddleware>();

app.MapFallbackToFile("index.html");

app.Run();
