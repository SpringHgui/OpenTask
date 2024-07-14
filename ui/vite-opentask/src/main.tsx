import React from 'react'
import ReactDOM from 'react-dom/client'
import '@/../app/globals.css'

import { RouterProvider } from 'react-router-dom'
import { createSyncStoragePersister } from '@tanstack/query-sync-storage-persister'
import { PersistQueryClientProvider } from '@tanstack/react-query-persist-client'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import { DevTools } from 'jotai-devtools'
import { TooltipProvider } from './components/ui/tooltip'
import { router } from './routes'
import { Loader2 } from 'lucide-react'
import { Toaster } from './components/ui/sonner'
import { queryClient } from './apis'
import { ThemeProvider } from './theme/theme-provider'
 
async function enableMocking() {
  console.log("[mode]", import.meta.env.MODE);
  if (import.meta.env.MODE !== 'mock') {
    return;
  }

  console.log(
    `%c Mocking start %c
  `,
    `padding-top: 0.5em; font-size: 2em;`,
    "padding-bottom: 0.5em;"
  );

  const { worker } = await import('./mocks/browser')

  // `worker.start()` returns a Promise that resolves
  // once the Service Worker is up and ready to intercept requests.
  return worker.start()
}

const persister = createSyncStoragePersister({
  storage: window.localStorage,
})

enableMocking().then(() => {
  ReactDOM.createRoot(document.getElementById('root')!).render(
    <React.StrictMode>
      <DevTools />
      <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
        <PersistQueryClientProvider client={queryClient} persistOptions={{ persister }}>
          <TooltipProvider>
            <RouterProvider future={{ v7_startTransition: true }} router={router} fallbackElement={<Loader2 className="m-auto h-6 w-6 animate-spin" />} />
          </TooltipProvider>
          <Toaster position="top-center" />
          <ReactQueryDevtools initialIsOpen />
        </PersistQueryClientProvider>
      </ThemeProvider>
    </React.StrictMode>
  )
})
