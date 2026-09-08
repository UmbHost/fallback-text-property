import { defineConfig } from "vite";

export default defineConfig({
  build: {
    lib: {
      entry: "src/index.ts",
      formats: ["es"],
      fileName: "fallback",
    },
    outDir: "../App_Plugins/FallbackTextstring/dist",
    emptyOutDir: true,
    rollupOptions: {
      // mustache is bundled; only the backoffice runtime is external.
      external: [/^@umbraco-cms\/backoffice/],
    },
  },
});
