import { jwtDecode } from "jwt-decode";

export const getToken = () => localStorage.getItem("accessToken");

export const parseToken = (token) => {
  try {
    return jwtDecode(token);
  } catch {
    return null;
  }
};

export const isTokenExpired = (decoded) => {
  if (!decoded?.exp) return true;
  return decoded.exp <= Math.floor(Date.now() / 1000);
};

export const getUserIdFromToken = () => {
  const token = getToken();
  if (!token) return null;
  try {
    const decoded = jwtDecode(token);
    return decoded?.sub || decoded?.userId || decoded?.id || null;
  } catch {
    return null;
  }
};

export const getAuthHeaders = () => ({
  Authorization: `Bearer ${getToken()}`,
});

export const getJsonAuthHeaders = () => ({
  Authorization: `Bearer ${getToken()}`,
  "Content-Type": "application/json",
});
