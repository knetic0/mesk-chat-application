import { UsersProvider } from '@/contexts/users-context';
import { createFileRoute, Outlet, redirect } from '@tanstack/react-router'

export const Route = createFileRoute('/_auth')({
  beforeLoad: ({ context }) => {
    if (!context.isAuthenticated) {
      throw redirect({ to: '/auth/login' });
    }
  },
  component: () => (
    <UsersProvider>
      <Outlet />
    </UsersProvider>
  ),
})