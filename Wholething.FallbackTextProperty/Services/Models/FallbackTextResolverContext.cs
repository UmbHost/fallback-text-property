using Umbraco.Cms.Core.Models.PublishedContent;

namespace Wholething.FallbackTextProperty.Services.Models
{
    public class FallbackTextResolverContext
    {
        public FallbackTextResolverContext(IPublishedElement element)
        {
            Element = element;
        }

        public IPublishedElement Element { get; set; }
        public IPublishedContent Content => Element as IPublishedContent;
    }
}
