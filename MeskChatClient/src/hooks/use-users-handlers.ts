import type { ApplicationUser2, ResponseEntityOfListOfApplicationUser } from '@/types';
import { useQueryClient } from '@tanstack/react-query';
import { useCallback } from 'react';

export const useUsersHandlers = () => {
  const queryClient = useQueryClient();

  const onStatusChange = useCallback(
    (applicationUser: ApplicationUser2) => {
      queryClient.setQueryData(
        ['users'],
        (oldData: ResponseEntityOfListOfApplicationUser | undefined) => {
          if (!oldData) return oldData;
          const exists = oldData.data?.some((u: ApplicationUser2) => u.id === applicationUser.id);
          return {
            ...oldData,
            data: exists
              ? oldData.data?.map((u: ApplicationUser2) =>
                  u.id === applicationUser.id ? { ...u, status: applicationUser.status } : u
                )
              : [...(oldData.data ?? []), applicationUser],
          };
        }
      );
    },
    [queryClient]
  );

  return { onStatusChange };
};
