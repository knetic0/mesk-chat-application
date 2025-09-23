import { UsersContext } from '@/contexts/users-context';
import { useContext } from 'react';

export const useUsers = () => {
  const context = useContext(UsersContext);
  if (!context) throw new Error('useUsers must be used within UsersProvider!');
  return context;
};
