import { createFileRoute, Outlet, useMatchRoute, useNavigate } from "@tanstack/react-router";
import { useEffect } from "react";
import { Search, MoreVertical } from "lucide-react";
import { useGetUsersQuery } from "@/features/queries/user/get-users/handler";
import type { ApplicationUser, ApplicationUser2, Message, ResponseEntityOfListOfApplicationUser, ResponseEntityOfListOfMessage } from "@/types";
import { useAuth } from "@/hooks/use-auth";
import { connection } from "@/signalr";
import { useLanguage } from "@/hooks/use-language";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useQueryClient } from "@tanstack/react-query";
import { STATUS_TEXT_MAP, STATUS_COLOR_MAP } from "@/lib/status";

export const Route = createFileRoute("/_auth/chat")({
  component: RouteComponent,
});

function RouteComponent() {
  const navigate = useNavigate();
  const { t } = useLanguage();
  const { user, logout } = useAuth();
  const matchRoute = useMatchRoute();
  const queryClient = useQueryClient();
  const match = matchRoute({ to: "/chat/$receiverId" as const });
  const receiverId = match && typeof match === "object" ? match.receiverId : undefined;

  const { data: users } = useGetUsersQuery();

  
  useEffect(() => {
    const recieveMessageHandler = (message: Message) => {
      if (message.senderId !== receiverId && message.receiverId !== receiverId) return;
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
    }

    const sentMessageHandler = (message: Message) => {
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
    }

    const statusChangeHandler = (applicationUser: ApplicationUser2) => {
      queryClient.setQueryData(
        ["users"],
        (oldData: ResponseEntityOfListOfApplicationUser | undefined) => {
          if (!oldData) return oldData;
          const exists = oldData.data?.some((u: ApplicationUser2) => u.id === applicationUser.id);
          return {
            ...oldData,
            data: exists
              ? oldData.data?.map((u: ApplicationUser2) =>
                  u.id === applicationUser.id ? { ...u, status: applicationUser.status } : u
                )
              : [...(oldData.data ?? []), applicationUser],
          };
        }
      );
    };
    (async () => {
      if (connection.state !== "Connected") {
        await connection.start();
      }
      connection.on("ReceiveMessage", recieveMessageHandler);
      connection.on("SentMessage", sentMessageHandler);
      connection.on("UserStatusChanged", statusChangeHandler);
    })();

    return () => {
      connection.off("ReceiveMessage", recieveMessageHandler);
      connection.off("SentMessage", sentMessageHandler);
      connection.off("UserStatusChanged", statusChangeHandler);
    };
  }, [queryClient, receiverId]);

  return (
    <div className="w-full h-[650px] max-w-5xl mx-auto bg-white rounded-2xl shadow-2xl flex overflow-hidden border border-gray-100 dark:bg-slate-900 dark:border-slate-800">
      <div className="w-1/3 bg-gradient-to-b from-blue-50 via-slate-50 to-indigo-100 dark:from-slate-900 dark:via-slate-800 dark:to-slate-900 border-r border-gray-100 dark:border-slate-800 flex flex-col">
        <div className="flex items-center justify-between p-4 border-b border-gray-100 dark:border-slate-800">
          <span className="font-bold text-xl">{t("chats")}</span>
          <MoreVertical className="text-gray-400 w-5 h-5" />
        </div>
        <div className="p-2">
          <div className="flex items-center bg-white dark:bg-slate-800 rounded-lg px-2 py-1 shadow-sm">
            <Search className="text-gray-400 mr-2 w-4 h-4" />
            <Input
              className="bg-transparent outline-none w-full text-sm"
              placeholder="Kullanıcı ara..."
            />
          </div>
        </div>
        <ul className="flex-1 overflow-y-auto mt-2">
          {users?.data?.map((item: ApplicationUser) => (
            <li
              key={item?.id}
              className={`cursor-pointer flex items-center gap-3 px-4 py-3 hover:bg-blue-100 dark:hover:bg-slate-800 transition-all ${
                receiverId === item?.id ? "bg-blue-50 dark:bg-slate-800" : ""
              }`}
              onClick={() =>
                navigate({
                  to: "/chat/$receiverId",
                  params: { receiverId: item?.id! },
                })
              }
            >
              <img
                src={"https://randomuser.me/api/portraits/men/3.jpg"}
                alt={item?.id}
                className="w-10 h-10 rounded-full object-cover border border-gray-200 dark:border-slate-700"
              />
              <div>
                <div className="font-medium text-base">
                  {item?.firstName + " " + item?.lastName}
                </div>
                {(item && item?.status !== undefined) && (
                  <div className="flex gap-1 items-center">
                    <div className={`w-2 h-2 rounded-full ${STATUS_COLOR_MAP[item.status]}`}></div>
                    <div className="text-xs text-gray-400">{STATUS_TEXT_MAP[item.status]}</div>
                  </div>
                )}
              </div>
            </li>
          ))}
        </ul>
        {user && (
          <div className="p-4 border-t border-gray-100 dark:border-slate-800 bg-white dark:bg-slate-900 flex items-center justify-between">
            <div className="flex items-center gap-3">
              <img
                src={"https://randomuser.me/api/portraits/men/5.jpg"}
                alt={user?.id}
                className="w-10 h-10 rounded-full object-cover border border-gray-200 dark:border-slate-700"
              />
              <div>
                <div className="font-medium text-base">
                  {user.firstName} {user.lastName}
                </div>
                <div className="text-xs text-gray-400">Ben</div>
              </div>
            </div>
            <Button
              variant="outline"
              size="sm"
              className="ml-2"
              onClick={() => {
                logout();
              }}
            >
              Çıkış
            </Button>
          </div>
        )}
      </div>
      <div className="flex-1 flex flex-col bg-gradient-to-br from-white via-blue-50 to-indigo-100 dark:from-slate-900 dark:via-slate-800 dark:to-slate-900">
        <Outlet />
      </div>
    </div>
  );
}
