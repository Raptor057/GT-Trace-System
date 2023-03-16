using GT.Trace.Common.CleanArch;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GT.Trace.EZ2000.Packaging.App
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            return services
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(InteractorPipeline<,>))
                .AddMediatR(typeof(ServiceCollectionEx).Assembly);
        }

    }
}