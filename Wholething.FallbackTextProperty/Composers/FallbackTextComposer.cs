using System.Collections.Generic;
using System.Linq;
using Wholething.FallbackTextProperty.Services;
using Wholething.FallbackTextProperty.Services.Impl;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Wholething.FallbackTextProperty.Composers
{
    public class FallbackTextComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddSingleton<IFallbackTextService, FallbackTextService>();
            builder.Services.AddSingleton<IFallbackTextReferenceParser, FallbackTextReferenceParser>();
            builder.Services.AddSingleton<IFallbackTextLoggerService, FallbackTextLoggerService>();

            builder.Services.AddSingleton<IFallbackTextResolver, ParentFallbackTextResolver>();
            builder.Services.AddSingleton<IFallbackTextResolver, RootFallbackTextResolver>();
            builder.Services.AddSingleton<IFallbackTextResolver, AncestorFallbackTextResolver>();
            builder.Services.AddSingleton<IFallbackTextResolver, UrlFallbackTextResolver>();
        }
    }
}
