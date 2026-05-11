import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import legacy from '@vitejs/plugin-legacy';

export default defineConfig({
  plugins: [
    react(),
    legacy({
      targets: ['Android >= 5', 'Chrome >= 60', 'Samsung >= 8', 'iOS >= 11'],
      additionalLegacyPolyfills: ['regenerator-runtime/runtime'],
    }),
  ],
  server: {
    port: 3000,
  },
  build: {
    outDir: 'build',
    sourcemap: false,
    target: 'es2015',
  },
});
