import axios, { type AxiosInstance } from "axios";
import { emitTokenRefreshed, emitTokenCleared } from './token-events';
import { getMeskChatApplicationWebApiV1 } from "./api/service";

const baseURL = import.meta.env.VITE_API_URL;

const api: AxiosInstance = axios.create({
    baseURL: baseURL,
});

api.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem("accessToken");
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    }
);

export const refreshTokenAsync = async (): Promise<{ accessToken: string }> => {
    const refreshToken = localStorage.getItem("refreshToken");
    if (!refreshToken) throw new Error("No refresh token");
    const client = getMeskChatApplicationWebApiV1();
    const response = await client.postApiV1AuthenticationRefreshToken({ refreshToken });
    const { accessToken, refreshToken: newRefreshToken } = response.data || {};
    if (accessToken && newRefreshToken) {
        emitTokenRefreshed(accessToken, newRefreshToken);
    }
    return { accessToken: accessToken! };
}

api.interceptors.response.use(
    response => response,
    async error => {
        const originalRequest = error.config;
        if (error.response?.status === 401 && !originalRequest._retry) {
            try {
                const { accessToken } = await refreshTokenAsync();
                originalRequest.headers.Authorization = `Bearer ${accessToken}`;
                originalRequest._retry = true;
                return api(originalRequest);
            } catch (refreshError) {
                emitTokenCleared();
            }
        }
        return Promise.reject(error);
    }
)

export const axiosInstance = async <TData>(config: any): Promise<TData> => (await api.request<TData>(config)).data;