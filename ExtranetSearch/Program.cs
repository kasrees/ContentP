using FullTextExtranetSearch.Configuration;
using Microsoft.OpenApi.Models;
using System.Reflection;
using NLog;
using Infrastructure.Logging;
using Infrastructure.Middleware;

var builder = WebApplication.CreateBuilder( args );

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.AddSingleton<ILog, NLogger>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Fulltext Extranet search API",
        Description = "An ASP.NET Core Web API for managing Fulltext Extranet search",
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Enter your ApiKey",
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[]{}
        }
    });
});

builder.Configuration.AddJsonFile( "pageconfig.json" );
builder.Configuration.AddJsonFile( "elksettings.json" );
builder.Services.ConfigureParser( builder.Configuration );
builder.Services.ConfigureContentCollector( builder.Configuration );
builder.Services.ConfigureElastic( builder.Configuration );
builder.Services.ConfigureIndexer( builder.Configuration );
builder.Services.ConfigureSearcher();

var app = builder.Build();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Run();
