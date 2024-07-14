import path from "path"
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react({
    "plugins": [
      // [
      //   "@swc-jotai/react-refresh",
      //   {}
      // ],
      // [
      //   "@swc-jotai/debug-label",
      //   {}
      // ]
    ]
  })],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
    },
  },
})
