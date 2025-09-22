import { useQueryClient } from "@tanstack/react-query"
import { useLanguage } from "./use-language";
import { useCallback } from "react";
import type { Message, ResponseEntityOfListOfMessage } from "@/types";
import { toast } from "sonner";

export const useMessageHandlers = (receiverId: string) => {
    const queryClient = useQueryClient();
    const { t } = useLanguage();

    const addMessageToCache = useCallback((message: Message) => {
        queryClient.setQueryData(
            ["messages", receiverId],
            (oldData: ResponseEntityOfListOfMessage | undefined) => {
            if (!oldData) return oldData;
            return {
                ...oldData,
                data: [...(oldData.data ?? []), message],
            };
            }
        );
    }, [queryClient, receiverId]);

    const onMessageReceived = useCallback((message: Message) => {
        if (message.senderId !== receiverId && message.receiverId !== receiverId) {
            const { firstName, lastName } = message.sender!;
            toast.info(t("chat.messageNotification", { firstName, lastName }), {
                description: `${message.text}`,
                duration: 5000,
            });
            return;
        };
        addMessageToCache(message);
    }, [addMessageToCache, receiverId, t]);

    const onMessageSent = useCallback((message: Message) => {
        addMessageToCache(message);
    }, [addMessageToCache]);

    return { onMessageReceived, onMessageSent };
}