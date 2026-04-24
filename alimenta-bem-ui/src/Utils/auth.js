import { jwtDecode } from "jwt-decode";

export const getToken = () => localStorage.getItem("accessToken");

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
