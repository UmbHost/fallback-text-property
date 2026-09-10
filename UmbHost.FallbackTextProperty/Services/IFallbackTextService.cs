using System;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace UmbHost.FallbackTextProperty.Services
{
    public interface IFallbackTextService
    {
        string BuildValue(IPublishedElement owner, IPublishedPropertyType propertyType, string? culture);

        // The new backoffice does not expose the dataType key to a property-editor UI, but the
        // editor always has its own fallback template (a config value). So the preview endpoint
        // sends the template directly rather than a dataTypeKey to look the config up.
        Dictionary<string, object> BuildDictionary(Guid nodeId, Guid? blockId, string? template, string? culture);
    }
}
