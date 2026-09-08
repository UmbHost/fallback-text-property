import { describe, it, expect } from "vitest";
import { renderFallback, preprocess } from "../src/fallback-preview";

describe("renderFallback", () => {
  it("interpolates own property", () => {
    expect(renderFallback("{{pageTitle}} - Bar", { pageTitle: "Welcome" })).toBe("Welcome - Bar");
  });

  it("rewrites numeric node id references to nodeNNNN keys", () => {
    const { template, dictionary } = preprocess("{{1104:heroHeader}}", { "1104:heroHeader": "Hi" });
    expect(template).toBe("{{node1104:heroHeader}}");
    expect(dictionary["node1104:heroHeader"]).toBe("Hi");
    expect(renderFallback("{{1104:heroHeader}}", { "1104:heroHeader": "Hi" })).toBe("Hi");
  });
});
