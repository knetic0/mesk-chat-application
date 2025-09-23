import ChangeLanguage from '@/components/change-language';
import ChangeTheme from '@/components/change-theme';
import type { AppRouterContext } from '@/router';
import { createRootRouteWithContext, Outlet } from '@tanstack/react-router';
import { TanStackRouterDevtools } from '@tanstack/react-router-devtools';

export const Route = createRootRouteWithContext<AppRouterContext>()({
  component: () => (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 via-blue-50 to-indigo-100 flex items-center justify-center relative p-4 dark:from-slate-900 dark:via-slate-800 dark:to-slate-900">
      <div className="absolute top-4 right-4 flex items-center space-x-3">
        <ChangeLanguage />
        <ChangeTheme />
      </div>
      <Outlet />
      <TanStackRouterDevtools />
    </div>
  ),
});
