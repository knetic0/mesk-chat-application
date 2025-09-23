import type { Message } from '@/types';

export const formatTime = (msg: Message) => {
  if (!msg || !msg.sendAt) return '';
  const date = new Date(msg.sendAt);
  return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
};
