import type { ResponseEntityOfLoginCommandResponse } from "@/types";
import { useMutation, type UseMutationOptions } from "@tanstack/react-query";
import { loginSchema, type LoginInput } from "./schema";
import { getMeskChatApplicationWebApiV1 } from "@/api/service";

export const useLoginMutation = (options?: Omit<UseMutationOptions<ResponseEntityOfLoginCommandResponse, Error, LoginInput, unknown>, 'mutationFn'>) => {
    return useMutation<ResponseEntityOfLoginCommandResponse, Error, LoginInput, unknown>({
        mutationFn: async (credentials: LoginInput) => {
            const validatedCredentials = loginSchema.parse(credentials);
            const client = getMeskChatApplicationWebApiV1();
            const response = await client.postApiV1AuthenticationLogin(validatedCredentials);
            return response;
        },
        ...options,
    })
}