using Hangfire;

namespace Scheduler
{
    public static class JobsHandler
    {
        public static void AddJobs()
        {
            RecurringJob.AddOrUpdate<JobsSpecifier>(x => x.ParseExtranet(), "0 3 * * *");
            RecurringJob.AddOrUpdate<JobsSpecifier>(x => x.HealthCheck(), "*/30 * * * *");
        }
    }
}
