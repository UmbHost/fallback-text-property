using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using UmbHost.FallbackTextProperty.Migrations;
using UmbHost.FallbackTextProperty.Services;
using UmbHost.FallbackTextProperty.Services.Impl;

namespace UmbHost.FallbackTextProperty.Composers
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

            // Injected into FallbackEditorUiAliasMigration (DI-activated migration).
            services.AddTransient<FallbackEditorUiAliasMigrator>();
            return services;
        }
    }

    public class FallbackTextComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            FallbackTextRegistrations.Add(builder.Services);
            // FallbackTextPropertyValueConverter : PropertyValueConverterBase : IDiscoverable,
            // and FallbackTextPropertyMigrationPlan : PackageMigrationPlan are auto-discovered by
            // Umbraco type scanning — the plan runs its pending step (the EditorUiAlias re-point)
            // once at startup. No explicit registration needed here.
        }
    }
}
