using GT.Trace.Common.CleanArch;
using GT.Trace.BomSnapShot.App;
using GT.Trace.BomSnapShot.Infra;
using MediatR;

namespace GT.Trace.BomSnapShotWebApi
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddSnapshotServices(this IServiceCollection services)
        {
            var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();

            return services
                .AddSingleton(config)
                .AddSnapshotInfraServices(config)
                .AddSingleton(typeof(ResultViewModel<>))
                .AddSingleton(typeof(GenericViewModel<>))
                .AddSnapshotAppServices()
                .AddMediatR(typeof(ServiceCollectionEx).Assembly);
        }
    }
}