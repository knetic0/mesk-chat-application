import type { ResponseEntityOfApplicationUser } from '@/types';
import { useQuery, type UseQueryOptions } from '@tanstack/react-query';
import { getMeskChatApplicationWebApiV1 } from '@/api/service';

export const useGetCurrentUserQuery = (
  options?: Omit<UseQueryOptions<ResponseEntityOfApplicationUser, Error>, 'queryKey' | 'queryFn'>
) => {
  return useQuery<ResponseEntityOfApplicationUser, Error>({
    queryKey: ['currentUser'],
    queryFn: async () => {
      const client = getMeskChatApplicationWebApiV1();
      const response = await client.getApiV1AuthenticationMe();
      return response;
    },
    ...options,
  });
};
