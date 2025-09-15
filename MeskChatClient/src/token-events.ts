export const TOKEN_EVENTS = {
  TOKEN_REFRESHED: 'tokenRefreshed',
  TOKEN_CLEARED: 'tokenCleared'
} as const;

export type TokenRefreshedEvent = CustomEvent<{
  accessToken: string;
  refreshToken: string;
}>;

export type TokenClearedEvent = CustomEvent<{}>;

export const emitTokenRefreshed = (accessToken: string, refreshToken: string) => {
  window.dispatchEvent(new CustomEvent(TOKEN_EVENTS.TOKEN_REFRESHED, {
    detail: { accessToken, refreshToken }
  }));
};

export const emitTokenCleared = () => {
  window.dispatchEvent(new CustomEvent(TOKEN_EVENTS.TOKEN_CLEARED));
};