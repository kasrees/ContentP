namespace Scheduler
{
    public class JobsSpecifier
    {
        private readonly ILogger<JobsSpecifier> _logger;
        public JobsSpecifier(ILogger<JobsSpecifier> logger)
        {
            _logger = logger;
        }

        public void HealthCheck()
        {
            _logger.LogInformation("Health check Hangfire");
        }

        public void ParseExtranet()
        {
            Console.WriteLine("Task executed");
        }
    }
}
