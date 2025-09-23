import { useQueryClient } from "@tanstack/react-query"
import { useLanguage } from "./use-language";
import { useCallback } from "react";
import type { Message, ResponseEntityOfListOfMessage } from "@/types";
import { toast } from "sonner";
import { HubConnectionState, type HubConnection } from "@microsoft/signalr";

export const useMessageHandlers = (receiverId: string, connection: HubConnection) => {
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

    const sendMessageAsync = useCallback(async (message: string) => {
        const _message = message?.trim();
        if(!_message) return { success: false, message: "Empty Message!" };
        if(!receiverId) return { success: false, message: "Invalid receiver!"};
        if(connection.state !== HubConnectionState.Connected) return { success: false, message: "Check your connection!"};
        const payload = { receiverId, message };
        try {
            await connection.send("SendMessageAsync", payload);
            return { success: true, message: "Message sended!"};
        } catch(error) {
            return { success: false, message: "Something went wrong while sending message!"};
        }
    }, [receiverId, connection])

    return { onMessageReceived, onMessageSent, sendMessageAsync };
}