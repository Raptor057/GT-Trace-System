using MediatR;
using GT.Trace.Common.CleanArch;
using GT.Trace.ConfigurationPanel.App;
using GT.Trace.ConfigurationPanel.Infra;

namespace GT.Trace.ConfigurationPanel.WebApi
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
                .AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionEx).Assembly);});
        }
    }
}
