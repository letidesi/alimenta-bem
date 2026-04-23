import React from "react";
import { Navigate } from "react-router-dom";

function parseJwt(token) {
  try {
    return JSON.parse(atob(token.split(".")[1]));
  } catch (e) {
    return null;
  }
}

export default function ProtectedLayoutRouter() {
  const token = localStorage.getItem("accessToken");

  if (!token) return <Navigate to="/login" />;

  const decoded = parseJwt(token);
  const role = Array.isArray(decoded?.roles) ? decoded.roles[0] : decoded?.roles;

  if (role?.toLowerCase() === "admin") {
    return <Navigate to="/admin" replace />;
  }

  if (role?.toLowerCase() === "citizen") {
    return <Navigate to="/logged-user" replace />;
  }

  return <Navigate to="/login" replace />;
}