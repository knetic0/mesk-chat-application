import { z } from 'zod';
import type { LoginCommand } from '@/types';

export const loginSchema = z.object({
  email: z.email('Invalid email address'),
  password: z
    .string()
    .min(6, { message: 'Password must be at least 6 characters long' })
    .regex(/[A-Z]/, { message: 'The password must include minimum 1 upper letter!' })
    .regex(/[a-z]/, { message: 'The password must include minimum 1 lower letter!' })
    .regex(/[0-9]/, { message: 'The password must include minimum 1 number!' })
    .regex(/[^a-zA-Z0-9]/, { message: 'The password must include minimum 1 special character!' }),
}) satisfies z.ZodType<LoginCommand>;

export type LoginInput = z.infer<typeof loginSchema>;
