using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;

namespace Wholething.FallbackTextProperty.PropertyEditors;

// v14+ split property editors into a client manifest (propertyEditorSchema/UI, for the
// backoffice) and a server IDataEditor (value storage + type). The client manifest alone
// leaves the editor alias unresolved server-side — which is why the 13->17 migration
// downgraded these data types to Umbraco.Plain.String. These DataEditors register the
// FallbackTextstring/FallbackTextarea aliases server-side with the correct value storage
// type (String=nvarchar / Text=ntext, matching the v9 DatabaseType) and are auto-discovered
// via type scanning. The FallbackValueConverter routes on the same aliases for rendering;
// config comes from the client-manifest settings (read as a dictionary).

[DataEditor(
    "FallbackTextstring",
    ValueType = ValueTypes.String,
    ValueEditorIsReusable = true)]
public class FallbackTextstringDataEditor : DataEditor
{
    public FallbackTextstringDataEditor(IDataValueEditorFactory dataValueEditorFactory)
        : base(dataValueEditorFactory)
    {
        SupportsReadOnly = true;
    }

    protected override IDataValueEditor CreateValueEditor()
        => DataValueEditorFactory.Create<TextOnlyValueEditor>(Attribute!);
}

[DataEditor(
    "FallbackTextarea",
    ValueType = ValueTypes.Text,
    ValueEditorIsReusable = true)]
public class FallbackTextareaDataEditor : DataEditor
{
    public FallbackTextareaDataEditor(IDataValueEditorFactory dataValueEditorFactory)
        : base(dataValueEditorFactory)
    {
        SupportsReadOnly = true;
    }

    protected override IDataValueEditor CreateValueEditor()
        => DataValueEditorFactory.Create<TextOnlyValueEditor>(Attribute!);
}
