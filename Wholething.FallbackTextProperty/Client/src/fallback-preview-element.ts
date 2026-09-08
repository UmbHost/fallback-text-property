import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";
import { property, state } from "@umbraco-cms/backoffice/external/lit";
import { UmbPropertyValueChangeEvent } from "@umbraco-cms/backoffice/property-editor";
import type {
  UmbPropertyEditorUiElement,
  UmbPropertyEditorConfigCollection,
} from "@umbraco-cms/backoffice/property-editor";
import { UMB_PROPERTY_DATASET_CONTEXT } from "@umbraco-cms/backoffice/property";
import { UMB_AUTH_CONTEXT } from "@umbraco-cms/backoffice/auth";
import { renderFallback } from "./fallback-preview.js";

/**
 * Shared base for the textstring/textarea fallback editors. Holds the value +
 * config, resolves the current node/culture from the property-dataset context,
 * and fetches the property dictionary from the backoffice endpoint to render the
 * Mustache fallback as placeholder text. Preview is best-effort: any failure
 * leaves the field fully usable with no preview.
 */
export abstract class FallbackPreviewElementBase
  extends UmbLitElement
  implements UmbPropertyEditorUiElement
{
  @property({ type: String })
  value = "";

  @property({ attribute: false })
  config?: UmbPropertyEditorConfigCollection;

  @state()
  protected _preview = "";

  #nodeId?: string;
  #culture?: string;
  #getToken?: () => Promise<string>;

  constructor() {
    super();

    this.consumeContext(UMB_PROPERTY_DATASET_CONTEXT, (ctx) => {
      this.#nodeId = ctx?.getUnique() ?? undefined;
      this.#culture = ctx?.getVariantId()?.culture ?? undefined;
      void this.#loadPreview();
    });

    this.consumeContext(UMB_AUTH_CONTEXT, (auth) => {
      this.#getToken = auth ? () => auth.getLatestToken() : undefined;
      void this.#loadPreview();
    });
  }

  protected override firstUpdated() {
    // config is assigned as a property after construction; try again once rendered.
    void this.#loadPreview();
  }

  protected get maxChars(): number | undefined {
    const n = Number(this.config?.getValueByAlias("maxChars"));
    return Number.isFinite(n) && n > 0 ? n : undefined;
  }

  protected get allowNone(): boolean {
    return this.config?.getValueByAlias("allowNone") === true;
  }

  protected get displayValue(): string {
    // Umbraco sets `value` to undefined/null for an empty property, which would
    // otherwise render the literal string "undefined" in the input.
    if (this.value == null || this.value === "<none>") return "";
    return this.value;
  }

  async #loadPreview() {
    const template = (this.config?.getValueByAlias("fallbackTemplate") as string) ?? "";
    if (!template || !this.#nodeId) return;

    const qs = new URLSearchParams({ nodeId: this.#nodeId, template });
    if (this.#culture) qs.set("culture", this.#culture);

    try {
      const headers: Record<string, string> = {};
      if (this.#getToken) {
        headers.Authorization = `Bearer ${await this.#getToken()}`;
      }
      const res = await fetch(`/umbraco/fallbacktext/dictionary?${qs}`, {
        credentials: "same-origin",
        headers,
      });
      if (!res.ok) return;
      const dict = (await res.json()) as Record<string, unknown>;
      this._preview = renderFallback(template, dict);
    } catch {
      // preview is best-effort — swallow and leave the field usable.
    }
  }

  protected onInputValue(newValue: string) {
    this.value = newValue;
    this.dispatchEvent(new UmbPropertyValueChangeEvent());
  }

  protected setNone() {
    this.value = "<none>";
    this.dispatchEvent(new UmbPropertyValueChangeEvent());
  }
}
