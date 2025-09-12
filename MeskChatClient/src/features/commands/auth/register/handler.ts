import type { ResponseEntityOfEmptyResponse } from "@/types";
import { useMutation, type UseMutationOptions } from "@tanstack/react-query";
import { registerSchema, type RegisterInput } from "./schema";
import { getMeskChatApplicationWebApiV1 } from "@/api/service";

export const useRegisterMutation = (options?: Omit<UseMutationOptions<ResponseEntityOfEmptyResponse, Error, RegisterInput, unknown>, 'mutationFn'>) => {
    return useMutation<ResponseEntityOfEmptyResponse, Error, RegisterInput, unknown>({
        mutationFn: async (credentials: RegisterInput) => {
            const validatedCredentials = registerSchema.parse(credentials);
            const client = getMeskChatApplicationWebApiV1();
            const response = await client.postApiV1AuthenticationRegister(validatedCredentials);
            return response;
        },
        ...options,
    })
}