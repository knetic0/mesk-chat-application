import * as signalR from '@microsoft/signalr';
import { refreshTokenAsync } from './axios';

const baseURL = import.meta.env.VITE_API_URL;

const getAuthHeaders = () => {
  return {
    Authorization: `Bearer ${localStorage.getItem('accessToken') || ''}`,
  };
};

class CustomHttpClient extends signalR.DefaultHttpClient {
  constructor() {
    super(console);
  }

  public async send(request: signalR.HttpRequest): Promise<signalR.HttpResponse> {
    const authHeaders = getAuthHeaders();
    request.headers = { ...request.headers, ...authHeaders };
    try {
      const response = await super.send(request);
      return response;
    } catch (er) {
      if (er instanceof signalR.HttpError) {
        const error = er as signalR.HttpError;
        if (error.statusCode === 401) {
          await refreshTokenAsync();
          const newAuthHeaders = getAuthHeaders();
          request.headers = { ...request.headers, ...newAuthHeaders };
        }
      } else {
        throw er;
      }
    }
    return super.send(request);
  }
}

export const connection = new signalR.HubConnectionBuilder()
  .withUrl(`${baseURL}/chat`, {
    httpClient: new CustomHttpClient(),
  })
  .withAutomaticReconnect()
  .build();
