import { createFileRoute, Outlet, useMatchRoute, useNavigate } from '@tanstack/react-router';
import { Search, MoreVertical } from 'lucide-react';
import type { ApplicationUser2 } from '@/types';
import { useAuth } from '@/hooks/use-auth';
import { useLanguage } from '@/hooks/use-language';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import StatusBadge from '@/components/status-badge';
import { SignalRProvider } from '@/contexts/signalr-connection-context';
import { useUsers } from '@/hooks/use-users';
import UsersSkeletonLoader from '@/components/users-skeleton-loader';
import { getProfilePhoto } from '@/lib/get-profile-photo';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import { Label } from '@/components/ui/label';
import { useUpdateProfilePhotoMutation } from '@/features/commands/user/update-photo/handler';
import { useQueryClient } from '@tanstack/react-query';

type MatchRoute = { receiverId: string };

export const Route = createFileRoute('/_auth/chat')({
  component: RouteComponent,
});

function RouteComponent() {
  const navigate = useNavigate();
  const { t } = useLanguage();
  const { user, logout } = useAuth();
  const matchRoute = useMatchRoute();
  const match = matchRoute({ to: '/chat/$receiverId' as const });
  const receiverId = (match as MatchRoute)?.receiverId;
  const { users, isPending } = useUsers();
  
  const queryClient = useQueryClient();
  const {
    mutate: handlePhotoUpload,
    isPending: isPhotoUploading,
  } = useUpdateProfilePhotoMutation({
    onSuccess: (data) => {
      queryClient.setQueryData(['currentUser'], (oldData: any) => {
        return {
          ...oldData,
          data: {
            ...oldData?.data,
            profilePhotoUrl: data.data,
          },
        };
      });
    }
  });

  return (
    <div className="w-full h-[650px] max-w-5xl mx-auto bg-white rounded-2xl shadow-2xl flex overflow-hidden border border-gray-100 dark:bg-slate-900 dark:border-slate-800">
      <div className="w-1/3 bg-gradient-to-b from-blue-50 via-slate-50 to-indigo-100 dark:from-slate-900 dark:via-slate-800 dark:to-slate-900 border-r border-gray-100 dark:border-slate-800 flex flex-col">
        <div className="flex items-center justify-between p-4 border-b border-gray-100 dark:border-slate-800">
          <span className="font-bold text-xl">{t('chats')}</span>
          <MoreVertical className="text-gray-400 w-5 h-5" />
        </div>
        <div className="p-2">
          <div className="flex items-center bg-white dark:bg-slate-800 rounded-lg px-2 py-1 shadow-sm">
            <Search className="text-gray-400 mr-2 w-4 h-4" />
            <Input
              className="bg-transparent outline-none w-full text-sm"
              placeholder={t('searchUsersPlaceholder')}
            />
          </div>
        </div>
        <ul className="flex-1 overflow-y-auto mt-2 overflow-x-hidden">
          {isPending
            ? Array.from({ length: 6 }).map((_, i: number) => (
              <li key={i} className="flex items-center gap-3 px-4 py-3">
                <UsersSkeletonLoader />
              </li>
            ))
            : users?.map((item: ApplicationUser2) => (
              <li
                key={item?.id}
                className={`cursor-pointer flex items-center gap-3 px-4 py-3 hover:bg-blue-100 dark:hover:bg-slate-800 transition-all ${receiverId === item?.id ? 'bg-blue-50 dark:bg-slate-800' : ''
                  }`}
                onClick={() =>
                  navigate({
                    to: '/chat/$receiverId',
                    params: { receiverId: item?.id! },
                  })
                }
              >
                <img
                  src={getProfilePhoto(item)}
                  alt={item?.id}
                  className="w-10 h-10 rounded-full object-cover border border-gray-200 dark:border-slate-700"
                />
                <div>
                  <div className="font-medium text-base">
                    {item?.firstName + ' ' + item?.lastName}
                  </div>
                  {item?.status !== undefined && <StatusBadge status={item.status} />}
                </div>
              </li>
            ))}
        </ul>
        {user && (
          <div className="p-4 border-t border-gray-100 dark:border-slate-800 bg-white dark:bg-slate-900 flex items-center justify-between">
            <div className="flex items-center gap-3">
              <Popover>
                <PopoverTrigger asChild>
                  <img
                    src={getProfilePhoto(user)}
                    className="w-10 h-10 rounded-full object-cover border border-gray-200 dark:border-slate-700 cursor-pointer"
                  />
                </PopoverTrigger>
                <PopoverContent className="w-80 bg-white dark:bg-slate-800 border-gray-200 dark:border-slate-700">
                  <div className="flex flex-col items-center gap-4">
                    <Label htmlFor="picture" className="text-gray-700 dark:text-gray-200">
                      {t("profilePhoto")}
                    </Label>
                    <input
                      id="picture"
                      type="file"
                      accept="image/*"
                      className="hidden"
                      onChange={(e) => {
                        const file = e.target.files?.[0];
                        if (file) {
                          handlePhotoUpload(file);
                        }
                      }}
                      disabled={isPhotoUploading}
                    />
                    <label htmlFor="picture" className="cursor-pointer">
                      <img
                        src={getProfilePhoto(user)}
                        className="w-32 h-32 rounded-full object-cover border-2 border-gray-300 dark:border-slate-600 hover:opacity-80 transition-opacity"
                        alt="Profile"
                      />
                    </label>
                    <p className="text-sm text-gray-500 dark:text-gray-400 text-center">
                      {isPhotoUploading ? t("loading") : t("clickToChangePhoto")}
                    </p>
                  </div>
                </PopoverContent>
              </Popover>
              <div>
                <div className="font-medium text-base">
                  {user.firstName} {user.lastName}
                </div>
                <div className="text-xs text-gray-400">{t('me')}</div>
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
              {t('logout')}
            </Button>
          </div>
        )}
      </div>
      <div className="flex-1 flex flex-col bg-gradient-to-br from-white via-blue-50 to-indigo-100 dark:from-slate-900 dark:via-slate-800 dark:to-slate-900">
        <SignalRProvider receiverId={receiverId}>
          <Outlet />
        </SignalRProvider>
      </div>
    </div>
  );
}
