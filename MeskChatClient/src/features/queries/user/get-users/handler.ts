import type { ResponseEntityOfListOfApplicationUser } from "@/types";
import { useQuery, type UseQueryOptions } from "@tanstack/react-query";
import { getMeskChatApplicationWebApiV1 } from "@/api/service";

export const useGetUsersQuery = (options?: Omit<UseQueryOptions<ResponseEntityOfListOfApplicationUser, Error>, 'queryKey' | 'queryFn'>) => {
    return useQuery<ResponseEntityOfListOfApplicationUser, Error>({
        queryKey: ['users'],
        queryFn: async () => {
            const client = getMeskChatApplicationWebApiV1();
            const response = await client.getApiV1Users();
            return response;
        },
        ...options,
    })
}