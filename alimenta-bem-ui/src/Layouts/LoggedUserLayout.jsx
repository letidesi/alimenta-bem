import React from "react";
import DashboardLayout from "./DashboardLayout";

export default function LoggedUserLayout() {
  const menuItems = [
    { key: "home", label: "Início", to: "/logged-user" },
    { key: "profile", label: "Completar perfil", to: "/logged-user/profile" },
    {
      key: "donation",
      label: "Realizar uma doação",
      to: "/logged-user/create-donation",
    },
  ];

  return <DashboardLayout menuItems={menuItems} roleLabel="Painel do Cidadão" />;
}
