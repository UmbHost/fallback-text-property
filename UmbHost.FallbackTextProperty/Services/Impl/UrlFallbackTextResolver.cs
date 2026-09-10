using System;
using UmbHost.FallbackTextProperty.Services.Models;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace UmbHost.FallbackTextProperty.Services.Impl
{
    public class UrlFallbackTextResolver : FallbackTextResolver
    {
        private readonly IPublishedContentCache _contentCache;
        private readonly IDocumentUrlService _documentUrlService;

        public UrlFallbackTextResolver(
            IFallbackTextLoggerService logger,
            IPublishedContentCache contentCache,
            IDocumentUrlService documentUrlService) : base(logger)
        {
            _contentCache = contentCache;
            _documentUrlService = documentUrlService;
        }

        protected override string FunctionName => "url";
        protected override bool RequireContent => false;

        public override void CheckArguments(string[] args, FallbackTextResolverContext context)
        {
            base.CheckArguments(args, context);

            if (args.Length != 1)
            {
                throw new ArgumentException("Expected exactly 1 argument");
            }
        }

        // v8 used publishedSnapshot.Content.GetByRoute(route). v14+ removed route lookup from
        // the content cache; IDocumentUrlService maps a route to a document key, then the cache
        // resolves the published content.
        protected override IPublishedContent? Resolve(string[] args, FallbackTextResolverContext context)
        {
            var key = _documentUrlService.GetDocumentKeyByRoute(args[0], culture: null, documentStartNodeId: null, isDraft: false);
            return key.HasValue ? _contentCache.GetById(false, key.Value) : null;
        }
    }
}
