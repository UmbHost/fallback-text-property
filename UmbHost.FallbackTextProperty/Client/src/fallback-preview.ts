import Mustache from "mustache";

/**
 * Mirror the server PreprocessTemplate/PreprocessKey: Mustache keys cannot start
 * with a digit, so a node-id reference "1104:heroHeader" -> "node1104:heroHeader"
 * on both the template and the dictionary keys.
 *
 * Note: function-reference keys (e.g. "ancestor(blogPost):companyName") are left
 * as-is here — the own-node and node-id preview cases are the common ones; the
 * authoritative render is always the server value converter.
 */
export function preprocess(template: string, dictionary: Record<string, unknown>) {
  const idRef = /\{\{(?:node)?([0-9]+):(\w+)\}\}/g;
  const outTemplate = template.replace(idRef, (_m, id, prop) => `{{node${id}:${prop}}}`);
  const outDict: Record<string, unknown> = {};
  for (const [k, v] of Object.entries(dictionary)) {
    outDict[/^[0-9]/.test(k) ? `node${k}` : k] = v;
  }
  return { template: outTemplate, dictionary: outDict };
}

export function renderFallback(template: string, dictionary: Record<string, unknown>): string {
  const p = preprocess(template, dictionary);
  Mustache.escape = (t) => t; // server HtmlDecodes; keep raw
  return Mustache.render(p.template, p.dictionary);
}
