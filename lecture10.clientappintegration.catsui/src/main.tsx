import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import App from './app/App'
import 'antd/dist/reset.css'
import './app/app.css'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'

const client = new QueryClient()

createRoot(document.getElementById('root')!).render(
    <BrowserRouter>
        <QueryClientProvider client={client}>
            <App />
        </QueryClientProvider>
    </BrowserRouter>
)
