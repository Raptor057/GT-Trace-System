using MediatR;
using Microsoft.Extensions.DependencyInjection;
using GT.Trace.Common.CleanArch;

namespace GT.Trace.Packaging.App
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(InteractorPipeline<,>))
                //.AddMediatR(typeof(ServiceCollectionEx).Assembly);
                .AddMediatR(cfg =>{cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionEx).Assembly);});
        }
    }
}