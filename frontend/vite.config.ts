import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  optimizeDeps: {
    // Explicitly include common deps so pre-bundling works reliably
    include: ['react', 'react-dom', 'react-router-dom', 'axios']
  }
  ,
  server: {
    // During development, proxy API calls to the Django backend
    proxy: {
      '/api': {
        target: 'http://127.0.0.1:8000',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})
