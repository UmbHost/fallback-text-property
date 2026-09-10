using System;
using UmbHost.FallbackTextProperty.Services;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;

namespace UmbHost.FallbackTextProperty.ValueConverters
{
    public class FallbackTextPropertyValueConverter : PropertyValueConverterBase
    {
        private readonly IFallbackTextService _fallbackTextService;
        private readonly IVariationContextAccessor _variationContextAccessor;

        public FallbackTextPropertyValueConverter(IFallbackTextService fallbackTextService, IVariationContextAccessor variationContextAccessor)
        {
            _fallbackTextService = fallbackTextService;
            _variationContextAccessor = variationContextAccessor;
        }

        public override bool IsConverter(IPublishedPropertyType propertyType)
        {
            return propertyType.EditorAlias == "FallbackTextstring" || propertyType.EditorAlias == "FallbackTextarea";
        }

        public override bool? IsValue(object? value, PropertyValueLevel level)
        {
            switch (level)
            {
                case PropertyValueLevel.Source:
                    return value != null && (!(value is string) || string.IsNullOrWhiteSpace((string)value) == false);
                default:
                    throw new NotSupportedException($"Invalid level: {level}.");
            }
        }

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
        {
            return typeof(string);
        }

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
        {
            return PropertyCacheLevel.Elements;
        }

        public override object? ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object? source,
            bool preview)
        {
            var value = source as string;

            if (value == "<none>")
            {
                return string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var culture = _variationContextAccessor?.VariationContext?.Culture;

            return _fallbackTextService.BuildValue(owner, propertyType, culture);
        }

        public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType,
            PropertyCacheLevel referenceCacheLevel, object? inter, bool preview)
        {
            return inter;
        }
    }
}
