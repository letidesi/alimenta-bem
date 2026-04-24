import React from "react";
import { BrowserRouter as Router, Routes, Route, Navigate} from "react-router-dom";

import PublicLayout from "./Layouts/PublicLayout";
import LoggedUserLayout from "./Layouts/LoggedUserLayout";
import AdminUserLayout from "./Layouts/AdminUserLayout";
import DeveloperUserLayout from "./Layouts/DeveloperUserLayout";

import Login from "./Components/Login";
import CreateUser from "./Components/CreateUser";
import CreateNaturalPerson from "./Components/CreateNaturalPerson";
import CreateOrganization from "./Components/CreateOrganization";
import CreateOrganizationRequirement from "./Components/CreateOrganizationRequirement";
import CreateDonation from "./Components/CreateDonation";
import UserHome from "./Components/UserHome";
import AdminHome from "./Components/AdminHome";
import DeveloperHome from "./Components/DeveloperHome";
import Profile from "./Components/Profile";

function parseJwt(token) {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      window
        .atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    return JSON.parse(jsonPayload);
  } catch (e) {
    return null;
  }
}

function normalizeRoles(decodedToken) {
  const rawRoles = decodedToken?.role || decodedToken?.roles || [];

  if (Array.isArray(rawRoles)) {
    return rawRoles.map((role) => String(role).toLowerCase());
  }

  if (typeof rawRoles === "string" && rawRoles.length > 0) {
    return [rawRoles.toLowerCase()];
  }

  return [];
}

function getDefaultRouteByRoles(roles) {
  if (roles.includes("admin")) return "/admin";
  if (roles.includes("developer")) return "/developer";
  if (roles.includes("citizen")) return "/logged-user";
  return "/login";
}

function isTokenExpired(decodedToken) {
  if (!decodedToken?.exp) return false;
  const nowInSeconds = Math.floor(Date.now() / 1000);
  return decodedToken.exp <= nowInSeconds;
}

function RequireAccess({ allowRoles = [], denyRoles = [], children }) {
  const token = localStorage.getItem("accessToken");

  if (!token) return <Navigate to="/login" replace />;

  const decoded = parseJwt(token);
  if (!decoded || isTokenExpired(decoded)) {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    return <Navigate to="/login" replace />;
  }

  const roles = normalizeRoles(decoded);
  const hasDeniedRole = denyRoles.some((role) => roles.includes(role.toLowerCase()));

  if (hasDeniedRole) {
    return <Navigate to={getDefaultRouteByRoles(roles)} replace />;
  }

  const hasAllowedRole = allowRoles.some((role) => roles.includes(role.toLowerCase()));

  if (!hasAllowedRole) {
    return <Navigate to={getDefaultRouteByRoles(roles)} replace />;
  }

  return children;
}

function App() {
  return (
    <Router>
      <Routes>
        <Route element={<PublicLayout />}>
          <Route path="/login" element={<Login />} />
          <Route path="/create-user" element={<CreateUser />} />
        </Route>

        <Route
          path="/admin"
          element={
            <RequireAccess allowRoles={["Admin"]}>
              <AdminUserLayout />
            </RequireAccess>
          }
        >
          <Route index element={<AdminHome />} />
          <Route path="profile" element={<Profile />} />
          <Route path="create-person" element={<CreateNaturalPerson />} />
          <Route path="create-organization" element={<CreateOrganization />} />
          <Route path="organization-req" element={<CreateOrganizationRequirement />} />
        </Route>

        <Route
          path="/logged-user"
          element={
            <RequireAccess allowRoles={["Citizen"]} denyRoles={["Admin", "Developer"]}>
              <LoggedUserLayout />
            </RequireAccess>
          }
        >
          <Route path="create-donation" element={<CreateDonation />} />
          <Route index element={<UserHome />} />
          <Route path="profile" element={<Profile />} />
        </Route>

        <Route
          path="/developer"
          element={
            <RequireAccess allowRoles={["Developer"]}>
              <DeveloperUserLayout />
            </RequireAccess>
          }
        >
          <Route index element={<DeveloperHome />} />
        </Route>

        <Route
          path="/protected"
          element={
            <RequireAccess allowRoles={["Admin"]}>
              <Navigate to="/admin" replace />
            </RequireAccess>
          }
        />

        <Route path="/" element={<Navigate to="/login" replace />} />
        <Route path="*" element={<Navigate to="/login" replace />} />
      </Routes>
    </Router>
  );
}

export default App;