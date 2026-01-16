import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';

// Use PORT env var if set, otherwise default to 5173 to avoid conflicts
const port = process.env.PORT ? parseInt(process.env.PORT, 10) : 5173;

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    server: {
        port,
    }
})