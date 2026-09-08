import { html, css, customElement } from "@umbraco-cms/backoffice/external/lit";
import { FallbackPreviewElementBase } from "./fallback-preview-element.js";

@customElement("fallback-textstring-ui")
export class FallbackTextstringUi extends FallbackPreviewElementBase {
  override render() {
    return html`
      <uui-input
        .value=${this.displayValue}
        placeholder=${this._preview}
        maxlength=${this.maxChars ?? ""}
        @input=${(e: InputEvent) => this.onInputValue((e.target as HTMLInputElement).value)}></uui-input>
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
    uui-input {
      width: 100%;
    }
  `;
}

declare global {
  interface HTMLElementTagNameMap {
    "fallback-textstring-ui": FallbackTextstringUi;
  }
}
