using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace Wholething.FallbackTextProperty.Services
{
    public interface IFallbackTextService
    {
        string BuildValue(IPublishedElement owner, IPublishedPropertyType propertyType, string culture);
        Dictionary<string, object> BuildDictionary(Guid nodeId, Guid? blockId, Guid dataTypeKey, string culture);
    }
}
