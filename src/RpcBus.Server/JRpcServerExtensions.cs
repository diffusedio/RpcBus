using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RpcBus.Server.Handlers;
using static RpcBus.Utils.JRpcUtils;

namespace RpcBus.Server
{
    public static class JRpcServerExtensions
    {
        public static void AddJRpcMediator(this IServiceCollection services, Assembly[] assemblies, Action<JRpcServerOptions>? setupAction = null)
        {
            // add configurators
            if (setupAction != null) services.Configure(setupAction);

            services.AddTransient<JRpcAuthenticationHandler>();
            services.AddTransient<JRpcAuthorizationHandler>();
            services.AddTransient<JRpcRequestHandler>();
            services.AddTransient<JRpcNotificationHandler>();

            foreach (var type in assemblies.SelectMany(a => a.DefinedTypes).Where(IsRequest))
            {
                JRpcMethods.Instance.TryAdd(GetMethod(type), type);
            }
        }

        public static void AddJRpcMediator(this IServiceCollection services, params Assembly[] assemblies)
            => AddJRpcMediator(services, assemblies);

        public static void AddJRpcMediator(this IServiceCollection services, Assembly assembly, Action<JRpcServerOptions>? setupAction = null)
            => AddJRpcMediator(services, new[] { assembly }, setupAction);

        public static IApplicationBuilder UseJRpcMediator(this IApplicationBuilder app)
        {
            return app.UseMiddleware<JRpcMiddleware>();
        }
    }
}
