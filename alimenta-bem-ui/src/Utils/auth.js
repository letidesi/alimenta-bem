import axios from "axios";

axios.defaults.withCredentials = true;

const SESSION_KEY = "session";

export const saveSession = ({ userId, role, expiresAt }) => {
  sessionStorage.setItem(SESSION_KEY, JSON.stringify({ userId, role, expiresAt }));
};

export const getSession = () => {
  try {
    return JSON.parse(sessionStorage.getItem(SESSION_KEY));
  } catch {
    return null;
  }
};

export const clearSession = () => {
  sessionStorage.removeItem(SESSION_KEY);
};

export const isSessionExpired = (session) => {
  if (!session?.expiresAt) return true;
  return session.expiresAt <= Math.floor(Date.now() / 1000);
};

export const getUserIdFromSession = () => getSession()?.userId ?? null;

export const getRoleFromSession = () => getSession()?.role ?? null;
