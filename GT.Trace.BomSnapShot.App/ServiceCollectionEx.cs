using MediatR;
using Microsoft.Extensions.DependencyInjection;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.BomSnapShot.App
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddSnapshotAppServices(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(InteractorPipeline<,>))
                //.AddMediatR(typeof(ServiceCollectionEx).Assembly);
                .AddMediatR(cfg =>{cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionEx).Assembly);});
        }
    }
}