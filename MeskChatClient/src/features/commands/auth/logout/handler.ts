import type { ResponseEntityOfEmptyResponse } from "@/types";
import { useMutation, type UseMutationOptions } from "@tanstack/react-query";
import { getMeskChatApplicationWebApiV1 } from "@/api/service";
import { logoutSchema, type LogoutInput } from "./schema";

export const useLogoutMutation = (options?: Omit<UseMutationOptions<ResponseEntityOfEmptyResponse, Error, LogoutInput, unknown>, 'mutationFn'>) => {
    return useMutation<ResponseEntityOfEmptyResponse, Error, LogoutInput, unknown>({
        mutationFn: async (credentials: LogoutInput) => {
            const validatedCredentials = logoutSchema.parse(credentials);
            const client = getMeskChatApplicationWebApiV1();
            const response = await client.postApiV1AuthenticationLogout(validatedCredentials);
            return response;
        },
        ...options,
    })
}