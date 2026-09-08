import type { UmbEntryPointOnInit } from "@umbraco-cms/backoffice/extension-api";
import "./fallback-textstring.element.js";
import "./fallback-textarea.element.js";

// The property-editor UIs register themselves as custom elements on import; the
// manifest points at their element names. Nothing else to do on init.
export const onInit: UmbEntryPointOnInit = () => {};
