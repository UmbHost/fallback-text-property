using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using UmbHost.FallbackTextProperty.Composers;
using UmbHost.FallbackTextProperty.Services;

namespace UmbHost.FallbackTextProperty.Test;

[TestFixture]
public class FallbackTextComposerTests
{
    // Assert the registrations are present rather than resolving the service — the
    // service's dependencies are Umbraco framework singletons not registered here.
    [Test]
    public void Add_RegistersServiceAndResolvers()
    {
        var services = new ServiceCollection();

        FallbackTextRegistrations.Add(services);

        Assert.That(services.Any(d => d.ServiceType == typeof(IFallbackTextService)), Is.True);
        Assert.That(services.Any(d => d.ServiceType == typeof(IFallbackTextReferenceParser)), Is.True);
        Assert.That(services.Any(d => d.ServiceType == typeof(IFallbackTextLoggerService)), Is.True);
        Assert.That(services.Count(d => d.ServiceType == typeof(IFallbackTextResolver)), Is.EqualTo(4));
    }
}
