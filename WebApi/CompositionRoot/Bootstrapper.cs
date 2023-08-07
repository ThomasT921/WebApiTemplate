using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using WebApi.Data;

namespace WebApi.CompositionRoot
{
    public class Bootstrapper
    {
        public static Container Bootstrap(IServiceCollection services, ConfigurationManager configuration)
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            container.Options.UseStrictLifestyleMismatchBehavior = true;

            services.AddMvc();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSimpleInjector(container, options =>
            {
                // AddAspNetCore() wraps web requests in a Simple Injector scope and
                // allows request-scoped framework services to be resolved.
                options.AddAspNetCore()

                    // Ensure activation of a specific framework type to be created by
                // Simple Injector instead of the built-in configuration system.
                // All calls are optional. You can enable what you need. For instance,
                // ViewComponents, PageModels, and TagHelpers are not needed when you
                // build a Web API.
                .AddControllerActivation()
                .AddViewComponentActivation();

                // Optionally, allow application components to depend on the non-generic
                // ILogger (Microsoft.Extensions.Logging) or IStringLocalizer
                // (Microsoft.Extensions.Localization) abstractions.
                //options.AddLogging();
                //options.AddLocalization();
            });

            container.RegisterSingleton(() =>
            {
                var options = container.GetInstance<IOptions<MvcNewtonsoftJsonOptions>>();
                return options.Value.SerializerSettings;
            });
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            container.BootstrapDb();
            return container;
        }
    }
}
