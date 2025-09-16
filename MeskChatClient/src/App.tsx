import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import { router } from "./router"
import { RouterProvider } from "@tanstack/react-router"
import ThemeProvider from "./contexts/theme-context"
import { AuthProvider } from "./contexts/auth-context"
import { useAuth } from "./hooks/use-auth"
import { Toaster } from "./components/ui/sonner"

declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 0,
    },
    mutations: {
      retry: 0,
    }
  },
})

function RouterProviderWithContext() {
  const auth = useAuth();

  return <RouterProvider router={router} context={{ ...auth }} />
}

export default function App() {
  return (
    <ThemeProvider defaultTheme="dark" storageKey="ui-theme">
      <QueryClientProvider client={queryClient}>
        <AuthProvider>
          <RouterProviderWithContext />
          <Toaster />
        </AuthProvider>
      </QueryClientProvider>
    </ThemeProvider>
  )
}