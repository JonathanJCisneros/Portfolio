using Serilog.Formatting.Json;
using Serilog.Sinks.AwsCloudWatch;
using Serilog.Events;
using Amazon.CloudWatchLogs;
using Serilog;

namespace PortfolioV2.Web.Custom_Code
{
    public class AWSConfig
    {
        public required string Id { get; set; }

        public required string Secret { get; set; }
    }

    public class LogInitializer
    {
        private readonly string _id;
        private readonly string _secret;

        public LogInitializer(string id, string secret)
        {
            _id = id;
            _secret = secret;
        }

        public void ConfigureLogger(string groupName)
        {
            CloudWatchSinkOptions options = new() 
            { 
                LogGroupName = groupName,
                TextFormatter = new JsonFormatter(Environment.NewLine),
                MinimumLogEventLevel = LogEventLevel.Verbose,
                BatchSizeLimit = 100,
                QueueSizeLimit = 10000,
                Period = TimeSpan.FromSeconds(10),
                CreateLogGroup = true,
                LogStreamNameProvider = new DefaultLogStreamProvider(),
                RetryAttempts = 0
            };

            AmazonCloudWatchLogsClient client = new(_id, _secret, Amazon.RegionEndpoint.USWest2);

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.AmazonCloudWatch(options, client)
                .CreateLogger();
        }
    }
}