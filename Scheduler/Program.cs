using Hangfire;
using Hangfire.SqlServer;
using Scheduler;
using Serilog;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

string logFilePath = builder.Configuration.GetValue<string>("Logging:HangfireHealthCheckLogsFilePath");
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Filter.ByIncludingOnly(Matching.FromSource<JobsSpecifier>())
    .WriteTo.RollingFile(logFilePath + "-{Date}.txt")
    .CreateLogger();

builder.Logging.AddSerilog();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var connectionString = builder.Configuration.GetConnectionString("HangfireConnection");
builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(connectionString, new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));

builder.Services.AddHangfireServer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseHangfireDashboard("/hangfire-dashboard");

JobsHandler.AddJobs();

app.Run();
