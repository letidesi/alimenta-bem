import React from "react";
import DashboardLayout from "./DashboardLayout";

export default function DeveloperUserLayout() {
  const menuItems = [
    { key: "home", label: "Início", to: "/developer" },
  ];

  return <DashboardLayout menuItems={menuItems} roleLabel="Painel do Desenvolvedor" />;
}
