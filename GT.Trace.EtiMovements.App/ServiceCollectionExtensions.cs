using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GT.Trace.EtiMovements.App
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(InteractorPipeline<,>))
                //.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);
                .AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly); });
        }
    }
}