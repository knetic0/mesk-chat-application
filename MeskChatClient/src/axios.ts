import axios, { type AxiosInstance } from "axios";
import { emitTokenRefreshed, emitTokenCleared } from './token-events';

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

api.interceptors.response.use(
    response => response,
    async error => {
        const originalRequest = error.config;
        if (error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;
            try {
                const refreshToken = localStorage.getItem("refreshToken");
                if (!refreshToken) throw new Error("No refresh token");
                const refreshResponse = await axios.post(
                    `${baseURL}/api/v1/authentication/refresh-token`,
                    { refreshToken }
                );
                const { newAccessToken, newRefreshToken } = refreshResponse.data?.data || {};
                if (newAccessToken && newRefreshToken) {
                    originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
                    emitTokenRefreshed(newAccessToken, newRefreshToken);
                    return api(originalRequest);
                }
            } catch (refreshError) {
                emitTokenCleared();
            }
        }
        return Promise.reject(error);
    }
)

export const axiosInstance = async <TData>(config: any): Promise<TData> => (await api.request<TData>(config)).data;