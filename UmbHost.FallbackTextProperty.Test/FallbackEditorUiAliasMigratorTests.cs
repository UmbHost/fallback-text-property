using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using UmbHost.FallbackTextProperty.Migrations;

namespace UmbHost.FallbackTextProperty.Test;

[TestFixture]
public class FallbackEditorUiAliasMigratorTests
{
    private static Mock<IDataType> Dt(string editorAlias, string? uiAlias)
    {
        var dt = new Mock<IDataType>();
        dt.SetupGet(d => d.EditorAlias).Returns(editorAlias);
        dt.SetupProperty(d => d.EditorUiAlias, uiAlias);
        return dt;
    }

    private static FallbackEditorUiAliasMigrator Migrator(IDataTypeService svc)
        => new(svc, Mock.Of<ILogger<FallbackEditorUiAliasMigrator>>());

    [Test]
    public async Task Repoints_BothSchemaAliases_FromLegacy()
    {
        var textstring = Dt("FallbackTextstring", "Wholething.PropertyEditorUi.FallbackTextstring");
        var textarea = Dt("FallbackTextarea", "Wholething.PropertyEditorUi.FallbackTextarea");
        var svc = new Mock<IDataTypeService>();
        svc.Setup(s => s.GetByEditorAliasAsync("FallbackTextstring")).ReturnsAsync(new[] { textstring.Object });
        svc.Setup(s => s.GetByEditorAliasAsync("FallbackTextarea")).ReturnsAsync(new[] { textarea.Object });

        var changed = await Migrator(svc.Object).RunAsync();

        Assert.That(textstring.Object.EditorUiAlias, Is.EqualTo("UmbHost.PropertyEditorUi.FallbackTextstring"));
        Assert.That(textarea.Object.EditorUiAlias, Is.EqualTo("UmbHost.PropertyEditorUi.FallbackTextarea"));
        Assert.That(changed, Is.EqualTo(2));
        svc.Verify(s => s.UpdateAsync(textstring.Object, It.IsAny<Guid>()), Times.Once);
        svc.Verify(s => s.UpdateAsync(textarea.Object, It.IsAny<Guid>()), Times.Once);
    }

    [Test]
    public async Task Repoints_NullUiAlias_ButSkips_WhenAlreadyNew()
    {
        var legacyNull = Dt("FallbackTextstring", null);
        var already = Dt("FallbackTextstring", "UmbHost.PropertyEditorUi.FallbackTextstring");
        var svc = new Mock<IDataTypeService>();
        svc.Setup(s => s.GetByEditorAliasAsync("FallbackTextstring")).ReturnsAsync(new[] { legacyNull.Object, already.Object });
        svc.Setup(s => s.GetByEditorAliasAsync("FallbackTextarea")).ReturnsAsync(Array.Empty<IDataType>());

        var changed = await Migrator(svc.Object).RunAsync();

        Assert.That(legacyNull.Object.EditorUiAlias, Is.EqualTo("UmbHost.PropertyEditorUi.FallbackTextstring"));
        Assert.That(changed, Is.EqualTo(1));
        svc.Verify(s => s.UpdateAsync(legacyNull.Object, It.IsAny<Guid>()), Times.Once);
        svc.Verify(s => s.UpdateAsync(already.Object, It.IsAny<Guid>()), Times.Never); // idempotent
    }
}
