using GT.Trace.Common.CleanArch;
using GT.Trace.EZ2000.Packaging.App;
using GT.Trace.EZ2000.Packaging.Infra;
using MediatR;

namespace GT.Trace.EZ2000.Packaging.UI.PackagingWebApi
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
                .AddMediatR(typeof(ServiceCollectionEx).Assembly);
        }
    }
}
