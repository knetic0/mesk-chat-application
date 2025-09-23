import { connection } from '@/signalr';
import { useEffect, useRef } from 'react';

const INACTIVITY_TIME = 2 * 60 * 1000; // 2 minutes

export const useAwayStatus = () => {
  const timeoutRef = useRef<NodeJS.Timeout | null>(null);
  const lastStatusRef = useRef<number | null>(null);

  useEffect(() => {
    const setStatus = (status: number) => {
      if (lastStatusRef.current !== status) {
        connection.invoke('UpdateUserStatusAsync', { status });
        lastStatusRef.current = status;
      }
    };

    const handleActivity = () => {
      setStatus(0);

      if (timeoutRef.current) {
        clearTimeout(timeoutRef.current);
      }

      timeoutRef.current = setTimeout(() => {
        setStatus(1);
      }, INACTIVITY_TIME);
    };

    window.addEventListener('mousemove', handleActivity);
    window.addEventListener('keydown', handleActivity);
    window.addEventListener('click', handleActivity);

    handleActivity();

    return () => {
      window.removeEventListener('mousemove', handleActivity);
      window.removeEventListener('keydown', handleActivity);
      window.removeEventListener('click', handleActivity);

      if (timeoutRef.current) {
        clearTimeout(timeoutRef.current);
      }
    };
  }, []);
};
