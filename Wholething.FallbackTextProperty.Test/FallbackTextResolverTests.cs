using Moq;
using NUnit.Framework;
using Umbraco.Cms.Core.Models.PublishedContent;
using Wholething.FallbackTextProperty.Services;
using Wholething.FallbackTextProperty.Services.Impl;
using Wholething.FallbackTextProperty.Services.Models;

namespace Wholething.FallbackTextProperty.Test;

[TestFixture]
public class FallbackTextResolverTests
{
    private static FallbackTextResolverContext Ctx()
        => new FallbackTextResolverContext(Mock.Of<IPublishedContent>());

    private static FallbackTextFunctionReference Ref(string function, string[] args, string key)
        => new FallbackTextFunctionReference(function, args, key);

    private static IFallbackTextLoggerService Logger() => Mock.Of<IFallbackTextLoggerService>();

    [Test]
    public void ParentResolver_CanResolve_ParentFunction()
    {
        var resolver = new ParentFallbackTextResolver(Logger());
        Assert.That(resolver.CanResolve(Ref("parent", Array.Empty<string>(), "parent:heroTitle"), Ctx()), Is.True);
        Assert.That(resolver.CanResolve(Ref("root", Array.Empty<string>(), "root:heroTitle"), Ctx()), Is.False);
    }

    [Test]
    public void AncestorResolver_CanResolve_AncestorFunction()
    {
        var resolver = new AncestorFallbackTextResolver(Logger());
        Assert.That(resolver.CanResolve(Ref("ancestor", new[] { "blogPost" }, "ancestor(blogPost):heroTitle"), Ctx()), Is.True);
    }
}
