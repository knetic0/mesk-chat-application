import type { ResponseEntityOfListOfMessage } from "@/types";
import { useQuery, type UseQueryOptions } from "@tanstack/react-query";
import { getMeskChatApplicationWebApiV1 } from "@/api/service";

export const useGetMessagesQuery = (receiverId: string | undefined, options?: Omit<UseQueryOptions<ResponseEntityOfListOfMessage, Error>, 'queryKey' | 'queryFn'>) => {
    return useQuery<ResponseEntityOfListOfMessage, Error>({
        queryKey: ['messages', receiverId],
        queryFn: async () => {
            const client = getMeskChatApplicationWebApiV1();
            const response = await client.getApiV1Messages({ receiverId: receiverId! });
            return response;
        },
        ...options,
    })
}