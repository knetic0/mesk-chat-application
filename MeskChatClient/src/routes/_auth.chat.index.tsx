import { createFileRoute } from "@tanstack/react-router";
import { MoreVertical } from "lucide-react";

export const Route = createFileRoute("/_auth/chat/")({
  component: RouteComponent,
});

function RouteComponent() {
  return (
    <>
      <div className="flex items-center gap-3 p-4 border-b border-gray-100 dark:border-slate-800 bg-white dark:bg-slate-900">
        <div className="w-10 h-10 rounded-full bg-gray-200 dark:bg-slate-700" />
        <div className="flex-1">
          <div className="font-semibold text-lg">Sohbet</div>
        </div>
        <MoreVertical className="text-gray-400 w-5 h-5" />
      </div>
      <div className="flex-1 overflow-y-auto p-6">
        <div className="flex items-center justify-center h-full text-gray-400 dark:text-gray-500 text-xl">
          Bir kullanıcı seçiniz ve sohbet başlatınız.
        </div>
      </div>
    </>
  );
}