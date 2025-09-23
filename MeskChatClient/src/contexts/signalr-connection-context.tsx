import { useAwayStatus } from '@/hooks/use-away-status';
import { useMessageHandlers } from '@/hooks/use-message-handlers';
import { useUsersHandlers } from '@/hooks/use-users-handlers';
import { connection } from '@/signalr';
import { HubConnectionState } from '@microsoft/signalr';
import { createContext, useEffect } from 'react';

interface SignalRContextType {
  sendMessageAsync: (message: string) => Promise<{ success: boolean; message: string }>;
}

export const SignalRContext = createContext<SignalRContextType | null>(null);

interface SignalRProviderProps {
  receiverId: string;
  children: React.ReactNode;
}

export const SignalRProvider: React.FC<SignalRProviderProps> = ({ receiverId, children }) => {
  const { onMessageReceived, onMessageSent, sendMessageAsync } = useMessageHandlers(
    receiverId,
    connection
  );
  const { onStatusChange } = useUsersHandlers();

  useEffect(() => {
    (async () => {
      if (connection.state !== HubConnectionState.Connected) {
        await connection.start();
      }

      connection.on('ReceiveMessage', onMessageReceived);
      connection.on('SentMessage', onMessageSent);
      connection.on('UserStatusChanged', onStatusChange);
    })();

    return () => {
      connection.off('ReceiveMessage', onMessageReceived);
      connection.off('SentMessage', onMessageSent);
      connection.off('UserStatusChanged', onStatusChange);
    };
  }, [onMessageReceived, onMessageSent, onStatusChange]);

  useAwayStatus();

  const values: SignalRContextType = {
    sendMessageAsync,
  };

  return <SignalRContext.Provider value={values}>{children}</SignalRContext.Provider>;
};
