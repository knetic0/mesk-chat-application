import { createRouter, Navigate } from '@tanstack/react-router';
import { routeTree } from './routeTree.gen';
import type { AuthContextType } from './contexts/auth-context';

export type AppRouterContext = AuthContextType & {};

export const router = createRouter({
  routeTree,
  defaultPreload: 'intent',
  defaultNotFoundComponent: () => <Navigate to="/not-found" replace />,
  context: {
    user: null,
    accessToken: null,
    refreshToken: null,
    isAuthenticated: false,
    login: () => {},
    logout: () => {},
  } as AppRouterContext,
});
