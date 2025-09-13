import { z } from "zod";
import type { LogoutCommand } from "@/types";

export const logoutSchema = z.object({
  refreshToken: z.string().nonoptional(),
}) satisfies z.ZodType<LogoutCommand>;

export type LogoutInput = z.infer<typeof logoutSchema>;