import React from "react";
import DashboardLayout from "./DashboardLayout";

export default function AdminUserLayout() {
  const menuItems = [
    { key: "home", label: "Início", to: "/admin" },
    { key: "profile", label: "Completar perfil", to: "/admin/profile" },
    {
      key: "org-create",
      label: "Registrar instituição",
      to: "/admin/create-organization",
    },
    {
      key: "org-req",
      label: "Necessidades de doações",
      to: "/admin/organization-req",
    },
    {
      key: "person-create",
      label: "Cadastrar doador",
      to: "/admin/create-person",
    },
  ];

  return <DashboardLayout menuItems={menuItems} roleLabel="Painel Administrativo" />;
}
