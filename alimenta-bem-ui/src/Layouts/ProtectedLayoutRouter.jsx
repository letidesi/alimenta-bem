import React from "react";
import { Navigate } from "react-router-dom";
import { getSession } from "../Utils/auth";

export default function ProtectedLayoutRouter() {
  const session = getSession();
  const role = session?.role?.toLowerCase();

  if (role === "admin") return <Navigate to="/admin" replace />;
  if (role === "developer") return <Navigate to="/developer" replace />;
  if (role === "citizen") return <Navigate to="/logged-user" replace />;

  return <Navigate to="/login" replace />;
}