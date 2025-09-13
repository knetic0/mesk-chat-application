import { useLogoutMutation } from "@/features/commands/auth/logout/handler";
import { useGetCurrentUserQuery } from "@/features/queries/auth/me/handler";
import { connection } from "@/signalr";
import type { ApplicationUser, ResponseEntityOfEmptyResponse } from "@/types";
import { createContext, useState } from "react";

export interface AuthContextType {
    user: ApplicationUser | null;
    accessToken: string | null;
    refreshToken: string | null;
    isAuthenticated: boolean;
    login: (accessToken: string, refreshToken: string) => void;
    logout: () => void;
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
    children: React.ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
    const [accessToken, setAccessToken] = useState<string | null>(localStorage.getItem("accessToken"));
    const [refreshToken, setRefreshToken] = useState<string | null>(localStorage.getItem("refreshToken"));

    const { mutate: logoutMutation } = useLogoutMutation({
        onSuccess: (data: ResponseEntityOfEmptyResponse) => {
            if(data.isSuccess) {
                setAccessToken(null);
                setRefreshToken(null);
                localStorage.removeItem("accessToken");
                localStorage.removeItem("refreshToken");
                connection.stop();
                window.location.href = "/auth/login";
            }
        }
    })
    const { data: user } = useGetCurrentUserQuery({ enabled: !!accessToken && !!refreshToken });

    const login = (accessToken: string, refreshToken: string) => {
        setAccessToken(accessToken);
        setRefreshToken(refreshToken);
        localStorage.setItem("accessToken", accessToken);
        localStorage.setItem("refreshToken", refreshToken);
    }

    const logout = () => logoutMutation({ refreshToken: refreshToken! });

    const isAuthenticated = !!accessToken && !!refreshToken;

    const value: AuthContextType = {
        user: user?.data || null,
        accessToken,
        refreshToken,
        isAuthenticated,
        login,
        logout,
    }

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    )
}