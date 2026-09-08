using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Wholething.FallbackTextProperty.Api;
using Wholething.FallbackTextProperty.Services;

namespace Wholething.FallbackTextProperty.Test;

[TestFixture]
public class FallbackTextControllerTests
{
    [Test]
    public void GetDictionary_ReturnsServiceResult()
    {
        var nodeId = Guid.NewGuid();
        const string template = "{{pageTitle}}";
        var svc = new Mock<IFallbackTextService>();
        svc.Setup(s => s.BuildDictionary(nodeId, null, template, "en-US"))
           .Returns(new Dictionary<string, object> { ["pageTitle"] = "Welcome" });

        var controller = new FallbackTextController(svc.Object);
        var result = controller.Dictionary(nodeId, template, "en-US", null) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        var dict = (IDictionary<string, object>)result!.Value!;
        Assert.That(dict["pageTitle"], Is.EqualTo("Welcome"));
    }
}
