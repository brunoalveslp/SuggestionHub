// vite.config.mts
import { defineConfig } from "vite";
import Vue from "@vitejs/plugin-vue";
import VueRouter from "unplugin-vue-router/vite";
import { VueRouterAutoImports } from "unplugin-vue-router";
import Layouts from "vite-plugin-vue-layouts-next";
import AutoImport from "unplugin-auto-import/vite";
import Components from "unplugin-vue-components/vite";
import Fonts from "unplugin-fonts/vite";
import Vuetify, { transformAssetUrls } from "vite-plugin-vuetify";
import tsconfigPaths from "vite-tsconfig-paths";
import { fileURLToPath, URL } from "node:url";

export default defineConfig({
  plugins: [
    tsconfigPaths(),
    VueRouter({ dts: "src/typed-router.d.ts" }),
    Layouts(),
    AutoImport({
      imports: [
        "vue",
        VueRouterAutoImports,
        { pinia: ["defineStore", "storeToRefs"] },
      ],
      dts: "src/auto-imports.d.ts",
      eslintrc: { enabled: true },
      vueTemplate: true,
    }),
    Components({ dts: "src/components.d.ts" }),
    Vue({ template: { transformAssetUrls } }),
    Vuetify({
      autoImport: true,
      styles: { configFile: "src/styles/settings.scss" },
    }),
    Fonts({
      fontsource: {
        families: [
          {
            name: "Roboto",
            weights: [100, 300, 400, 500, 700, 900],
            styles: ["normal", "italic"],
          },
        ],
      },
    }),
  ],
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
    extensions: [".mjs", ".js", ".ts", ".vue", ".json"],
  },
  optimizeDeps: {
    exclude: [
      "vuetify",
      "vue-router",
      "unplugin-vue-router/runtime",
      "unplugin-vue-router/data-loaders",
      "unplugin-vue-router/data-loaders/basic",
    ],
  },
  server: {
    port: 3000,
  },
  css: {
    preprocessorOptions: {
      sass: { api: "modern-compiler" },
      scss: { api: "modern-compiler" },
    },
  },
  define: { "process.env": {} },
});
