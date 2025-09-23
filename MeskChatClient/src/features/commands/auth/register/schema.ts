import { z } from 'zod';
import type { RegisterCommand } from '@/types';

export const registerSchema = z.object({
  firstName: z
    .string()
    .min(3, { message: 'First name must be at least 3 characters long' })
    .max(30, { message: 'First name must be at most 30 characters long' }),
  lastName: z
    .string()
    .min(3, { message: 'Last name must be at least 3 characters long' })
    .max(30, { message: 'Last name must be at most 30 characters long' }),
  username: z
    .string()
    .min(3, { message: 'Username must be at least 3 characters long' })
    .max(20, { message: 'Username must be at most 20 characters long' }),
  email: z.email('Invalid email address'),
  password: z
    .string()
    .min(6, { message: 'Password must be at least 6 characters long' })
    .regex(/[A-Z]/, { message: 'The password must include minimum 1 upper letter!' })
    .regex(/[a-z]/, { message: 'The password must include minimum 1 lower letter!' })
    .regex(/[0-9]/, { message: 'The password must include minimum 1 number!' })
    .regex(/[^a-zA-Z0-9]/, { message: 'The password must include minimum 1 special character!' }),
}) satisfies z.ZodType<RegisterCommand>;

export type RegisterInput = z.infer<typeof registerSchema>;
