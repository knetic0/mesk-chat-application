import type { IFormFile, ResponseEntityOfstring } from '@/types';
import { useMutation, type UseMutationOptions } from '@tanstack/react-query';
import { getMeskChatApplicationWebApiV1 } from '@/api/service';

export const useUpdateProfilePhotoMutation = (
  options?: Omit<
    UseMutationOptions<ResponseEntityOfstring, Error, IFormFile, unknown>,
    'mutationFn'
  >
) => {
  return useMutation<ResponseEntityOfstring, Error, IFormFile, unknown>({
    mutationFn: async (credentials: IFormFile) => {
      const client = getMeskChatApplicationWebApiV1();
      const response = await client.putApiV1UsersUpdateProfilePhoto({ photo: credentials });
      return response;
    },
    ...options,
  });
};
