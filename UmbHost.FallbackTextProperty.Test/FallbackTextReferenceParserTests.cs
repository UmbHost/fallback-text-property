using NUnit.Framework;
using UmbHost.FallbackTextProperty.Services;
using UmbHost.FallbackTextProperty.Services.Impl;

namespace UmbHost.FallbackTextProperty.Test;

[TestFixture]
public class FallbackTextReferenceParserTests
{
    private IFallbackTextReferenceParser _referenceParser = null!;

    [SetUp]
    public void SetUp()
    {
        _referenceParser = new FallbackTextReferenceParser();
    }

    [Test]
    public void InvalidTemplate()
    {
        var template = "This is a {{1234:test}} template";
        var references = _referenceParser.Parse(template);

        Assert.That(references, Is.Empty);
    }

    [Test]
    public void ValidTemplate()
    {
        var template = "This is a {{ancestor(blogPost):companyName}} template This is a {{ancestor(1, 3, 4):companyAddress}} template";
        var references = _referenceParser.Parse(template);

        Assert.That(references.Count, Is.EqualTo(2));

        Assert.That(references[0].Function, Is.EqualTo("ancestor"));
        Assert.That(references[0].Args.Length, Is.EqualTo(1));
        Assert.That(references[0].Args[0], Is.EqualTo("blogPost"));

        Assert.That(references[1].Function, Is.EqualTo("ancestor"));
        Assert.That(references[1].Args.Length, Is.EqualTo(3));
        Assert.That(references[1].Args[0], Is.EqualTo("1"));
    }
}
