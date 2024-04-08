using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GT.Trace.Common.Logging
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddLoggingServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            return services
                .AddLogging(logging =>
                {
                    var section = configuration.GetSection("CustomLogging");
                    var serilogLogger = new LoggerConfiguration()
                        .Enrich.WithProperty("Project", section.GetSection("Project").Value)
                        .WriteTo.Seq(
                            serverUrl: section.GetSection("SeqUri").Value ?? "",
                            restrictedToMinimumLevel: (LogEventLevel)Enum.Parse(typeof(LogEventLevel), section.GetSection("LogEventLevel").Value ?? ""))
                        .CreateLogger();
                    logging.AddSerilog(logger: serilogLogger, dispose: true);
                });
        }
    }
}