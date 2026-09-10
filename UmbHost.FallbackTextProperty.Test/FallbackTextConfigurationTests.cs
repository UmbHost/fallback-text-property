using NUnit.Framework;
using UmbHost.FallbackTextProperty.Configuration;

namespace UmbHost.FallbackTextProperty.Test;

[TestFixture]
public class FallbackTextConfigurationTests
{
    [Test]
    public void Defaults_AreSafe()
    {
        var config = new FallbackTextConfiguration();
        Assert.That(config.FallbackTemplate, Is.Null);
        Assert.That(config.AllowNone, Is.False);
        Assert.That(config.Rows, Is.Null);
    }

    [Test]
    public void From_MapsDictionary()
    {
        var config = FallbackTextConfiguration.From(new Dictionary<string, object>
        {
            ["fallbackTemplate"] = "{{pageTitle}}",
            ["maxChars"] = 120,
            ["allowNone"] = true,
            ["rows"] = 5,
        });
        Assert.That(config.FallbackTemplate, Is.EqualTo("{{pageTitle}}"));
        Assert.That(config.MaxChars, Is.EqualTo(120));
        Assert.That(config.AllowNone, Is.True);
        Assert.That(config.Rows, Is.EqualTo(5));
    }

    [Test]
    public void From_Null_ReturnsDefaults()
    {
        var config = FallbackTextConfiguration.From(null);
        Assert.That(config.FallbackTemplate, Is.Null);
        Assert.That(config.AllowNone, Is.False);
    }
}
