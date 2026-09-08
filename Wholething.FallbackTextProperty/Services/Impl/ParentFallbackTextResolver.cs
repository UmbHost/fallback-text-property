using System;
using Wholething.FallbackTextProperty.Services.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Wholething.FallbackTextProperty.Services.Impl
{
    public class ParentFallbackTextResolver : FallbackTextResolver
    {
        public ParentFallbackTextResolver(IFallbackTextLoggerService logger) : base(logger)
        {
        }


        protected override string FunctionName => "parent";
        protected override bool RequireContent => true;

        public override void CheckArguments(string[] args, FallbackTextResolverContext context)
        {
            base.CheckArguments(args, context);

            if (args.Length > 0)
            {
                throw new ArgumentException("Did not expect any arguments");
            }
        }

        protected override IPublishedContent? Resolve(string[] args, FallbackTextResolverContext context)
        {
            // TODO v18: IPublishedContent.Parent is [Obsolete] (removal v18) -> use Parent<T>()/navigation.
            return context.Content?.Parent;
        }
    }
}
