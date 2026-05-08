import React from "react";
import { BrowserRouter as Router, Routes, Route, Navigate} from "react-router-dom";
import { getSession, isSessionExpired, clearSession } from "./Utils/auth";

import PublicLayout from "./Layouts/PublicLayout";
import LoggedUserLayout from "./Layouts/LoggedUserLayout";
import AdminUserLayout from "./Layouts/AdminUserLayout";
import DeveloperUserLayout from "./Layouts/DeveloperUserLayout";

import Login from "./Components/Login";
import CreateUser from "./Components/CreateUser";
import ForgotPassword from "./Components/ForgotPassword";
import ResetPassword from "./Components/ResetPassword";
import CreateNaturalPerson from "./Components/CreateNaturalPerson";
import CreateOrganization from "./Components/CreateOrganization";
import CreateOrganizationRequirement from "./Components/CreateOrganizationRequirement";
import CreateDonation from "./Components/CreateDonation";
import UserHome from "./Components/UserHome";
import AdminHome from "./Components/AdminHome";
import DeveloperHome from "./Components/DeveloperHome";
import Profile from "./Components/Profile";

function getDefaultRouteByRole(role) {
  if (!role) return "/login";
  const r = role.toLowerCase();
  if (r === "admin") return "/admin";
  if (r === "developer") return "/developer";
  if (r === "citizen") return "/logged-user";
  return "/login";
}

function RequireAccess({ allowRoles = [], denyRoles = [], children }) {
  const session = getSession();

  if (!session || isSessionExpired(session)) {
    clearSession();
    return <Navigate to="/login" replace />;
  }

  const role = session.role?.toLowerCase() ?? "";

  if (denyRoles.some((r) => r.toLowerCase() === role)) {
    return <Navigate to={getDefaultRouteByRole(session.role)} replace />;
  }

  if (!allowRoles.some((r) => r.toLowerCase() === role)) {
    return <Navigate to={getDefaultRouteByRole(session.role)} replace />;
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
          <Route path="/forgot-password" element={<ForgotPassword />} />
          <Route path="/reset-password" element={<ResetPassword />} />
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