import React from "react";
import { Navigate } from "react-router-dom";
import { parseToken } from "../Utils/auth";

function getHighestPriorityRole(roles) {
  const rolePriority = ["admin", "developer", "citizen"];
  return rolePriority.find((role) => roles.includes(role)) || null;
}

export default function ProtectedLayoutRouter() {
  const token = localStorage.getItem("accessToken");

  if (!token) return <Navigate to="/login" />;

  const decoded = parseToken(token);
  const rawRoles = decoded?.role || decoded?.roles;
  const normalizedRoles = Array.isArray(rawRoles)
    ? rawRoles.map((role) => String(role).toLowerCase())
    : rawRoles
      ? [String(rawRoles).toLowerCase()]
      : [];
  const role = getHighestPriorityRole(normalizedRoles);

  if (role === "admin") {
    return <Navigate to="/admin" replace />;
  }

  if (role === "developer") {
    return <Navigate to="/developer" replace />;
  }

  if (role === "citizen") {
    return <Navigate to="/logged-user" replace />;
  }

  return <Navigate to="/login" replace />;
}