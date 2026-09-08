using Wholething.FallbackTextProperty.Services.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Wholething.FallbackTextProperty.Services
{
    public interface IFallbackTextResolver
    {
        bool CanResolve(FallbackTextFunctionReference reference, FallbackTextResolverContext context);
        IPublishedContent Resolve(FallbackTextFunctionReference reference, FallbackTextResolverContext context);
        void CheckArguments(string[] args, FallbackTextResolverContext context);
    }
}
