import { useGetUsersQuery } from '@/features/queries/user/get-users/handler';
import { type ApplicationUser2 } from '@/types';
import { createContext } from 'react';

interface UsersContextType {
  users: ApplicationUser2[];
  isPending: boolean;
}

export const UsersContext = createContext<UsersContextType | undefined>(undefined);

interface UsersProviderProps {
  children: React.ReactNode;
}

export const UsersProvider: React.FC<UsersProviderProps> = ({ children }) => {
  const { data, isPending } = useGetUsersQuery();
  const users = data?.data!;

  const values = {
    users,
    isPending,
  };

  return <UsersContext.Provider value={values}>{children}</UsersContext.Provider>;
};
