using MediatR;
using GT.Trace.TraceabilityLegacyIntegration.App;
using GT.Trace.Common.CleanArch;
using GT.Trace.TraceabilityLegacyIntegration.Infra;

namespace GT.Trace.TraceabilityLegacyIntegrationWebApi
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();

            return services
                .AddSingleton(config)
                .AddInfraServices(config)
                .AddSingleton(typeof(ResultViewModel<>))
                .AddSingleton(typeof(GenericViewModel<>))
                .AddAppServices()
                .AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionEx).Assembly); });
        }
    }
}
