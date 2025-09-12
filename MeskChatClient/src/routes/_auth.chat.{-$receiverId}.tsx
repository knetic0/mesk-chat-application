import { createFileRoute, useNavigate, useParams } from "@tanstack/react-router";
import { useEffect, useState } from "react";
import { Search, Send, MoreVertical } from "lucide-react";
import { useGetUsersQuery } from "@/features/queries/user/get-users/handler";
import type { ApplicationUser, Message } from "@/types";
import { useGetMessagesQuery } from "@/features/queries/chat/get-messages/handler";
import { useAuth } from "@/hooks/use-auth";
import { connection } from "@/signalr";
import { useLanguage } from "@/hooks/use-language";

export const Route = createFileRoute("/_auth/chat/{-$receiverId}")({
  component: RouteComponent,
});

function RouteComponent() {
  const navigate = useNavigate();
  const { t } = useLanguage();
  const { user } = useAuth();
  const { receiverId } = useParams({ from: "/_auth/chat/{-$receiverId}" });
  const [input, setInput] = useState("");

  const { data: users } = useGetUsersQuery();
  const selectedUser = users?.data?.find((u: ApplicationUser) => u?.id === receiverId);

  const [liveMessages, setLiveMessages] = useState<Message[]>([]);

  const { data: messages } = useGetMessagesQuery(receiverId, {
    enabled: !!receiverId,
  });

  const currentUserId = user?.id;
  
  useEffect(() => {
    const start = async () => {
      if (connection.state !== "Disconnected") return;
      try {
        await connection.start();
        console.log("SignalR connected ✅");

        connection.on("ReceiveMessage", (message: Message) => {
          if(message.senderId === receiverId || message.receiverId === receiverId)
            setLiveMessages((prev) => [...prev, message]);
        });

        connection.on("SentMessage", (message: Message) => {
          if(message.senderId === receiverId || message.receiverId === receiverId)
            setLiveMessages((prev) => [...prev, message]);
        });

      } catch (err) {
        console.error("SignalR connection error:", err);
      }
    };

    start();

    return () => {
      connection.off("ReceiveMessage");
      connection.off("SentMessage");
    };
  }, []);
  
  const send = () => {
    if (input.trim() && receiverId) {
      const payload = {
        receiverId: receiverId,
        message: input.trim(),
      };
      connection.invoke("SendMessageAsync", payload)
        .then(() => setInput(""))
        .catch(err => console.error("Send message error: ", err));
      }
  }

  const allMessages = messages?.data ? [...messages.data, ...liveMessages] : liveMessages;

  return (
    <div className="w-full h-[650px] max-w-5xl mx-auto bg-white rounded-2xl shadow-2xl flex overflow-hidden border border-gray-100 dark:bg-slate-900 dark:border-slate-800">
      {/* Sidebar */}
      <div className="w-1/3 bg-gradient-to-b from-blue-50 via-slate-50 to-indigo-100 dark:from-slate-900 dark:via-slate-800 dark:to-slate-900 border-r border-gray-100 dark:border-slate-800 flex flex-col">
        <div className="flex items-center justify-between p-4 border-b border-gray-100 dark:border-slate-800">
          <span className="font-bold text-xl">{t("chats")}</span>
          <MoreVertical className="text-gray-400 w-5 h-5" />
        </div>
        <div className="p-2">
          <div className="flex items-center bg-white dark:bg-slate-800 rounded-lg px-2 py-1 shadow-sm">
            <Search className="text-gray-400 mr-2 w-4 h-4" />
            <input
              className="bg-transparent outline-none w-full text-sm"
              placeholder="Kullanıcı ara..."
            />
          </div>
        </div>
        <ul className="flex-1 overflow-y-auto mt-2">
          {users?.data?.map((user: ApplicationUser) => (
            <li
              key={user?.id}
              className={`cursor-pointer flex items-center gap-3 px-4 py-3 hover:bg-blue-100 dark:hover:bg-slate-800 transition-all ${
                receiverId === user?.id ? "bg-blue-50 dark:bg-slate-800" : ""
              }`}
              onClick={() =>
                navigate({
                  to: "/chat/{-$receiverId}",
                  params: { receiverId: user?.id! },
                })
              }
            >
              <img
                src={"https://randomuser.me/api/portraits/men/3.jpg"}
                alt={user?.id}
                className="w-10 h-10 rounded-full object-cover border border-gray-200 dark:border-slate-700"
              />
              <div>
                <div className="font-medium text-base">
                  {user?.firstName + " " + user?.lastName}
                </div>
                <div className="text-xs text-gray-400">{user?.status}</div>
              </div>
            </li>
          ))}
        </ul>
      </div>

      {/* Chat Panel */}
      <div className="flex-1 flex flex-col bg-gradient-to-br from-white via-blue-50 to-indigo-100 dark:from-slate-900 dark:via-slate-800 dark:to-slate-900">
        {/* Header */}
        <div className="flex items-center gap-3 p-4 border-b border-gray-100 dark:border-slate-800 bg-white dark:bg-slate-900">
          {receiverId ? (
            <img
              src={"https://randomuser.me/api/portraits/men/3.jpg"}
              alt="avatar"
              className="w-10 h-10 rounded-full object-cover border border-gray-200 dark:border-slate-700"
            />
          ) : (
            <div className="w-10 h-10 rounded-full bg-gray-200 dark:bg-slate-700" />
          )}
          <div className="flex-1">
            <div className="font-semibold text-lg">
              {receiverId
                ? `${selectedUser?.firstName} ${selectedUser?.lastName}`
                : "Sohbet"}
            </div>
            <div className="text-xs text-gray-400">
              {receiverId ? "Çevrimiçi" : "Kullanıcı seçilmedi"}
            </div>
          </div>
          <MoreVertical className="text-gray-400 w-5 h-5" />
        </div>

        {/* Messages */}
        <div className="flex-1 overflow-y-auto p-6 space-y-3">
          {receiverId ? (
            allMessages.map((msg: Message, idx: number) => {
              const isMine = msg.senderId === currentUserId;
              return (
                <div
                  key={msg.id ?? idx}
                  className={`flex ${isMine ? "justify-end" : "justify-start"}`}
                >
                  <div
                    className={`relative px-5 py-3 rounded-2xl max-w-md shadow-sm ${
                      isMine
                        ? "bg-blue-600 text-white"
                        : "bg-white dark:bg-slate-800 text-gray-900 dark:text-white"
                    }`}
                  >
                    <span className="block text-base">{msg.text}</span>
                  </div>
                </div>
              );
            })
          ) : (
            <div className="flex items-center justify-center h-full text-gray-400 dark:text-gray-500 text-xl">
              Bir kullanıcı seçiniz ve sohbet başlatınız.
            </div>
          )}
        </div>

        {/* Input */}
        <div className="p-4 border-t border-gray-100 dark:border-slate-800 bg-white dark:bg-slate-900">
          {receiverId ? (
            <form
              className="flex gap-2 items-center"
              onSubmit={(e) => {
                e.preventDefault();
                send();
              }}
            >
              <input
                className="flex-1 px-4 py-3 rounded-full border border-gray-200 dark:border-slate-700 focus:outline-none focus:ring-2 focus:ring-blue-200 dark:bg-slate-800 dark:text-white text-base shadow-sm"
                type="text"
                placeholder="Mesaj yaz..."
                value={input}
                onChange={(e) => setInput(e.target.value)}
              />
              <button
                type="submit"
                className="p-3 bg-blue-600 text-white rounded-full hover:bg-blue-700 flex items-center justify-center shadow-md"
                title="Gönder"
              >
                <Send className="w-5 h-5" />
              </button>
            </form>
          ) : null}
        </div>
      </div>
    </div>
  );
}
