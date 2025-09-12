import { createRouter, Navigate } from "@tanstack/react-router";
import { routeTree } from "./routeTree.gen";
import type { AuthContextType } from "./contexts/auth-context";

export const router = createRouter({
  routeTree,
  defaultPreload: 'intent',
  defaultNotFoundComponent: () => <Navigate to='/not-found' replace />,
  context: {} as AuthContextType,
});