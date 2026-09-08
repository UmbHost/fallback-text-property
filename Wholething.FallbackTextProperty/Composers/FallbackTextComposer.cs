using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Wholething.FallbackTextProperty.Services;
using Wholething.FallbackTextProperty.Services.Impl;

namespace Wholething.FallbackTextProperty.Composers
{
    // Pure IServiceCollection registrations, extracted so they can be unit-tested
    // without an IUmbracoBuilder. IPublishedContentCache/IDocumentUrlService/
    // IDocumentNavigationQueryService are framework singletons, so transient
    // consumers here carry no captive-dependency risk.
    public static class FallbackTextRegistrations
    {
        public static IServiceCollection Add(IServiceCollection services)
        {
            services.AddTransient<IFallbackTextService, FallbackTextService>();
            services.AddTransient<IFallbackTextReferenceParser, FallbackTextReferenceParser>();
            services.AddSingleton<IFallbackTextLoggerService, FallbackTextLoggerService>();

            services.AddTransient<IFallbackTextResolver, ParentFallbackTextResolver>();
            services.AddTransient<IFallbackTextResolver, RootFallbackTextResolver>();
            services.AddTransient<IFallbackTextResolver, AncestorFallbackTextResolver>();
            services.AddTransient<IFallbackTextResolver, UrlFallbackTextResolver>();
            return services;
        }
    }

    public class FallbackTextComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            FallbackTextRegistrations.Add(builder.Services);
            // FallbackTextPropertyValueConverter : PropertyValueConverterBase : IDiscoverable,
            // so it is auto-registered by Umbraco type scanning — no explicit Append needed.
        }
    }
}
