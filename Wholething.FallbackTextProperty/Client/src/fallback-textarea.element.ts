import { html, css, customElement } from "@umbraco-cms/backoffice/external/lit";
import { FallbackPreviewElementBase } from "./fallback-preview-element.js";

@customElement("fallback-textarea-ui")
export class FallbackTextareaUi extends FallbackPreviewElementBase {
  private get rows(): number {
    const n = Number(this.config?.getValueByAlias("rows"));
    return Number.isFinite(n) && n > 0 ? n : 10;
  }

  override render() {
    return html`
      <uui-textarea
        rows=${this.rows}
        .value=${this.displayValue}
        placeholder=${this._preview}
        maxlength=${this.maxChars ?? ""}
        @input=${(e: InputEvent) => this.onInputValue((e.target as HTMLTextAreaElement).value)}></uui-textarea>
      ${this.allowNone
        ? html`<uui-button
            look="secondary"
            label="None"
            @click=${() => this.setNone()}></uui-button>`
        : ""}
    `;
  }

  static override styles = css`
    :host {
      display: block;
    }
    uui-textarea {
      width: 100%;
    }
  `;
}

declare global {
  interface HTMLElementTagNameMap {
    "fallback-textarea-ui": FallbackTextareaUi;
  }
}
