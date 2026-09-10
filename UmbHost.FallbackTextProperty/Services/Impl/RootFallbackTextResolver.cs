using System;
using System.Linq;
using UmbHost.FallbackTextProperty.Services.Models;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Extensions;

namespace UmbHost.FallbackTextProperty.Services.Impl
{
    public class RootFallbackTextResolver : FallbackTextResolver
    {
        private readonly IPublishedContentCache _contentCache;
        private readonly IDocumentNavigationQueryService _navigationQueryService;

        public RootFallbackTextResolver(
            IFallbackTextLoggerService logger,
            IPublishedContentCache contentCache,
            IDocumentNavigationQueryService navigationQueryService) : base(logger)
        {
            _contentCache = contentCache;
            _navigationQueryService = navigationQueryService;
        }

        protected override string FunctionName => "root";
        protected override bool RequireContent => false;

        public override void CheckArguments(string[] args, FallbackTextResolverContext context)
        {
            base.CheckArguments(args, context);

            if (args.Length > 0)
            {
                throw new ArgumentException("Did not expect any arguments");
            }
        }

        // v8 used publishedSnapshot.Content.GetAtRoot().First(): the first top-level node in
        // the content tree, independent of the current node (works from within blocks too).
        // v14+ has no snapshot; TryGetRootKeys gives the root document keys in tree order.
        protected override IPublishedContent? Resolve(string[] args, FallbackTextResolverContext context)
        {
            if (!_navigationQueryService.TryGetRootKeys(out var rootKeys))
            {
                return null;
            }

            foreach (var key in rootKeys)
            {
                var node = _contentCache.GetById(false, key);
                if (node != null)
                {
                    return node;
                }
            }

            return null;
        }
    }
}
