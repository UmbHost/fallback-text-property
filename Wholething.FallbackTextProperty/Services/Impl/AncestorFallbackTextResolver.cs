using System;
using Wholething.FallbackTextProperty.Services.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Wholething.FallbackTextProperty.Services.Impl
{
    public class AncestorFallbackTextResolver : FallbackTextResolver
    {
        public AncestorFallbackTextResolver(IFallbackTextLoggerService logger) : base(logger)
        {
        }

        protected override string FunctionName => "ancestor";
        protected override bool RequireContent => true;

        public override void CheckArguments(string[] args, FallbackTextResolverContext context)
        {
            base.CheckArguments(args, context);

            if (args.Length > 1)
            {
                throw new ArgumentException($"{FunctionName} expects 0 or 1 arguments");
            }
        }

        protected override IPublishedContent? Resolve(string[] args, FallbackTextResolverContext context)
        {
            if (context.Content == null)
            {
                return null;
            }
            if (args.Length == 1)
            {
                return context.Content.Ancestor(args[0]);
            }
            return context.Content.Ancestor();
        }
    }
}
