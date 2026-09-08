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
    public async Task GetDictionary_ReturnsServiceResult()
    {
        var nodeId = Guid.NewGuid();
        var dtKey = Guid.NewGuid();
        var svc = new Mock<IFallbackTextService>();
        svc.Setup(s => s.BuildDictionaryAsync(nodeId, null, dtKey, "en-US"))
           .ReturnsAsync(new Dictionary<string, object> { ["pageTitle"] = "Welcome" });

        var controller = new FallbackTextController(svc.Object);
        var result = await controller.Dictionary(nodeId, dtKey, "en-US", null) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        var dict = (IDictionary<string, object>)result!.Value!;
        Assert.That(dict["pageTitle"], Is.EqualTo("Welcome"));
    }
}
