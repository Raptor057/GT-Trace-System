using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GT.Trace.Etis.App
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(InteractorPipeline<,>))
                //.AddMediatR(typeof(ServiceCollectionEx).Assembly);
                .AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionEx).Assembly); });
        }
    }
}