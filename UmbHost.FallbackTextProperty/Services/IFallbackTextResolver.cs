using UmbHost.FallbackTextProperty.Services.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace UmbHost.FallbackTextProperty.Services
{
    public interface IFallbackTextResolver
    {
        bool CanResolve(FallbackTextFunctionReference reference, FallbackTextResolverContext context);
        IPublishedContent? Resolve(FallbackTextFunctionReference reference, FallbackTextResolverContext context);
        void CheckArguments(string[] args, FallbackTextResolverContext context);
    }
}
