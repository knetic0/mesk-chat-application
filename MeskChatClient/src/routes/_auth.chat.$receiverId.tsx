import { createFileRoute, useParams } from '@tanstack/react-router';
import { useEffect, useRef, useState } from 'react';
import { Send, MoreVertical } from 'lucide-react';
import type { ApplicationUser, Message } from '@/types';
import { useGetMessagesQuery } from '@/features/queries/chat/get-messages/handler';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { useAuth } from '@/hooks/use-auth';
import { useLanguage } from '@/hooks/use-language';
import StatusBadge from '@/components/status-badge';
import { useSignalR } from '@/hooks/use-signalr';
import { formatTime } from '@/lib/format-time';
import { useUsers } from '@/hooks/use-users';

export const Route = createFileRoute('/_auth/chat/$receiverId')({
  component: RouteComponent,
});

function RouteComponent() {
  const { t } = useLanguage();
  const { user } = useAuth();
  const { receiverId } = useParams({ from: '/_auth/chat/$receiverId' });
  const [input, setInput] = useState('');
  const messagesEndRef = useRef<HTMLDivElement | null>(null);
  const { users } = useUsers();
  const selectedUser = users?.find((u: ApplicationUser) => u?.id === receiverId);
  const { data: messages } = useGetMessagesQuery(receiverId, { enabled: !!receiverId });
  const { sendMessageAsync } = useSignalR();

  const handleSendMessageAsync = () => {
    sendMessageAsync(input).then(response => {
      if (response.success) {
        setInput('');
      }
    });
  };

  const scrollToBottom = () => messagesEndRef?.current?.scrollIntoView({ behavior: 'smooth' });
  useEffect(() => scrollToBottom(), [messages]);

  return (
    <>
      <div className="flex items-center gap-3 p-4 border-b border-gray-100 dark:border-slate-800 bg-white dark:bg-slate-900">
        <img
          src={'https://randomuser.me/api/portraits/men/3.jpg'}
          alt="avatar"
          className="w-10 h-10 rounded-full object-cover border border-gray-200 dark:border-slate-700"
        />
        <div className="flex-1">
          <div className="font-semibold text-lg">
            {selectedUser ? `${selectedUser.firstName} ${selectedUser.lastName}` : 'Sohbet'}
          </div>
          {selectedUser?.status !== undefined && <StatusBadge status={selectedUser.status} />}
        </div>
        <MoreVertical className="text-gray-400 w-5 h-5" />
      </div>
      <div className="flex-1 overflow-y-auto p-6 space-y-3">
        {messages?.data?.map((msg: Message, idx: number) => {
          const isMine = msg.senderId === user?.id;
          const time = formatTime(msg);
          return (
            <div key={msg.id ?? idx} className={`flex ${isMine ? 'justify-end' : 'justify-start'}`}>
              <div className={`max-w-md flex flex-col ${isMine ? 'items-end' : 'items-start'}`}>
                <div
                  className={`px-5 py-3 rounded-2xl shadow-sm ${isMine
                    ? 'bg-blue-600 text-white'
                    : 'bg-white dark:bg-slate-800 text-gray-900 dark:text-white'
                    }`}
                >
                  <span className="block text-base">{msg.text}</span>
                </div>
                {time && (
                  <span
                    className={`mt-1 text-[11px] leading-none text-gray-300 ${isMine ? 'text-right pr-1' : 'text-left pl-1'}`}
                  >
                    {time}
                  </span>
                )}
              </div>
            </div>
          );
        })}
        <div ref={messagesEndRef} />
      </div>
      <div className="p-4 border-t border-gray-100 dark:border-slate-800 bg-white dark:bg-slate-900">
        <form
          className="flex gap-2 items-center"
          onSubmit={e => {
            e.preventDefault();
            handleSendMessageAsync();
          }}
        >
          <Input
            className="flex-1 px-4 py-3 rounded-full border border-gray-200 dark:border-slate-700 focus:outline-none focus:ring-2 focus:ring-blue-200 dark:bg-slate-800 dark:text-white text-base shadow-sm"
            type="text"
            placeholder={t('chat.writeMessage')}
            value={input}
            onChange={e => setInput(e.target.value)}
          />
          <Button
            type="submit"
            className="p-3 bg-blue-600 text-white rounded-full hover:bg-blue-700 flex items-center justify-center shadow-md"
            title="Gönder"
          >
            <Send className="w-5 h-5" />
          </Button>
        </form>
      </div>
    </>
  );
}
