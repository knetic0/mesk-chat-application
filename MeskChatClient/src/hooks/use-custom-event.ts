import { useEffect } from 'react';

type CustomEventDetail<T> = T extends CustomEvent<infer U> ? U : never;

export const useCustomEvent = <T extends CustomEvent<any>>(
  eventType: string,
  handler: (detail: CustomEventDetail<T>) => void,
  dependencies: React.DependencyList = []
) => {
  useEffect(() => {
    const eventHandler = (event: Event) => {
      const customEvent = event as T;
      handler(customEvent.detail);
    };

    window.addEventListener(eventType, eventHandler);

    return () => {
      window.removeEventListener(eventType, eventHandler);
    };
  }, dependencies);
};
