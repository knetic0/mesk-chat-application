// src/lib/signalr.ts
import * as signalR from "@microsoft/signalr";

const baseURL = import.meta.env.VITE_API_URL;

export const connection = new signalR.HubConnectionBuilder()
  .withUrl(`${baseURL}/chat`, {
    accessTokenFactory: () => localStorage.getItem("accessToken") || "",
  })
  .withAutomaticReconnect()
  .build();
