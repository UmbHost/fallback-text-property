using Moq;
using NUnit.Framework;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using UmbHost.FallbackTextProperty.Configuration;
using UmbHost.FallbackTextProperty.Services;
using UmbHost.FallbackTextProperty.Services.Impl;

namespace UmbHost.FallbackTextProperty.Test;

[TestFixture]
public class FallbackTextServiceTests
{
    private static IPublishedProperty StringProp(string alias, string value)
    {
        // GetSourceValueWithCulture branches on PropertyType.VariesByCulture()
        // (Umbraco.Extensions reads IPublishedPropertyType.Variations).
        var propType = new Mock<IPublishedPropertyType>();
        propType.SetupGet(pt => pt.Variations).Returns(ContentVariation.Nothing);

        var prop = new Mock<IPublishedProperty>();
        prop.SetupGet(p => p.Alias).Returns(alias);
        prop.SetupGet(p => p.PropertyType).Returns(propType.Object);
        prop.Setup(p => p.GetSourceValue(It.IsAny<string?>(), It.IsAny<string?>())).Returns(value);
        return prop.Object;
    }

    [Test]
    public void BuildDictionary_IncludesOwnStringProperties()
    {
        var owner = new Mock<IPublishedContent>();
        owner.SetupGet(o => o.Name).Returns("Home");
        owner.SetupGet(o => o.Properties).Returns(new[] { StringProp("pageTitle", "Welcome") });

        var contentCache = new Mock<IPublishedContentCache>();
        var logger = new Mock<IFallbackTextLoggerService>();
        var parser = new Mock<IFallbackTextReferenceParser>();
        parser.Setup(p => p.Parse(It.IsAny<string>()))
              .Returns(new List<Services.Models.FallbackTextFunctionReference>());

        var service = new FallbackTextService(
            contentCache.Object, Array.Empty<IFallbackTextResolver>(),
            parser.Object, logger.Object);

        var dict = service.BuildDictionary(owner.Object,
            new FallbackTextConfiguration { FallbackTemplate = "{{pageTitle}}" },
            culture: null);

        Assert.That(dict["pageTitle"], Is.EqualTo("Welcome"));
        Assert.That(dict["name"], Is.EqualTo("Home"));
    }
}
