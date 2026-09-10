using HandlebarsDotNet;
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
    private static FallbackTextService NewService() => new(
        Mock.Of<IPublishedContentCache>(),
        Array.Empty<IFallbackTextResolver>(),
        Mock.Of<IFallbackTextReferenceParser>(),
        Mock.Of<IFallbackTextLoggerService>());

    private static string Token(string template) => template.Replace("{{", string.Empty).Replace("}}", string.Empty);

    // A GUID node reference must survive preprocessing as a Handlebars-safe token that matches
    // its dictionary key (regression: hyphens + a "node" prefix on the key only used to mismatch,
    // so {{<guid>:alias}} threw HandlebarsUndefinedBindingException).
    [Test]
    public void GuidReference_TemplateTokenMatchesKey_AndRenders()
    {
        var svc = NewService();
        const string guid = "531fd84f-5aaa-4ccf-aff0-29b22f433005";

        var template = svc.PreprocessTemplate($"{{{{{guid}:wSettingsTitleSuffix}}}}");
        var dict = svc.PreprocessDictionary(new Dictionary<string, object>
        {
            [$"{guid}:wSettingsTitleSuffix"] = " - suffix"
        });
        var token = Token(template);

        Assert.That(token, Does.Not.Contain("-"), "GUID token still contains hyphens (not Handlebars-safe)");
        Assert.That(dict.ContainsKey(token), Is.True, $"token '{token}' not in [{string.Join(", ", dict.Keys)}]");
        Assert.That(Handlebars.Compile(template)(dict), Is.EqualTo(" - suffix"));
    }

    [Test]
    public void IntegerReference_StillAlignsAndRenders()
    {
        var svc = NewService();

        var template = svc.PreprocessTemplate("{{1207:wSettingsTitleSuffix}}");
        var dict = svc.PreprocessDictionary(new Dictionary<string, object>
        {
            ["1207:wSettingsTitleSuffix"] = " - suffix"
        });

        Assert.That(Token(template), Is.EqualTo("node1207:wSettingsTitleSuffix"));
        Assert.That(dict.ContainsKey(Token(template)), Is.True);
        Assert.That(Handlebars.Compile(template)(dict), Is.EqualTo(" - suffix"));
    }

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
