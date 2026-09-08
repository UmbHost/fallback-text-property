using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace Wholething.FallbackTextProperty.Services
{
    public interface IFallbackTextService
    {
        string BuildValue(IPublishedElement owner, IPublishedPropertyType propertyType, string? culture);
        Task<Dictionary<string, object>> BuildDictionaryAsync(Guid nodeId, Guid? blockId, Guid dataTypeKey, string? culture);
    }
}
