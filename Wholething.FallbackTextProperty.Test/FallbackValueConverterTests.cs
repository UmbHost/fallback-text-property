using Moq;
using NUnit.Framework;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Wholething.FallbackTextProperty.Services;
using Wholething.FallbackTextProperty.ValueConverters;

namespace Wholething.FallbackTextProperty.Test;

[TestFixture]
public class FallbackValueConverterTests
{
    private static FallbackTextPropertyValueConverter Build(Mock<IFallbackTextService> svc)
        => new(svc.Object, Mock.Of<IVariationContextAccessor>());

    private static IPublishedPropertyType PropType()
    {
        var pt = new Mock<IPublishedPropertyType>();
        pt.SetupGet(p => p.EditorAlias).Returns("FallbackTextstring");
        return pt.Object;
    }

    [Test]
    public void NonEmptyValue_WinsWithoutCallingService()
    {
        var svc = new Mock<IFallbackTextService>();
        var result = Build(svc).ConvertSourceToIntermediate(Mock.Of<IPublishedElement>(), PropType(), "typed", false);
        Assert.That(result, Is.EqualTo("typed"));
        svc.Verify(s => s.BuildValue(It.IsAny<IPublishedElement>(), It.IsAny<IPublishedPropertyType>(), It.IsAny<string?>()), Times.Never);
    }

    [Test]
    public void NoneSentinel_BecomesEmpty()
    {
        var svc = new Mock<IFallbackTextService>();
        var result = Build(svc).ConvertSourceToIntermediate(Mock.Of<IPublishedElement>(), PropType(), "<none>", false);
        Assert.That(result, Is.EqualTo(string.Empty));
    }

    [Test]
    public void Empty_UsesServiceFallback()
    {
        var svc = new Mock<IFallbackTextService>();
        svc.Setup(s => s.BuildValue(It.IsAny<IPublishedElement>(), It.IsAny<IPublishedPropertyType>(), It.IsAny<string?>())).Returns("fallback!");
        var result = Build(svc).ConvertSourceToIntermediate(Mock.Of<IPublishedElement>(), PropType(), null, false);
        Assert.That(result, Is.EqualTo("fallback!"));
    }
}
