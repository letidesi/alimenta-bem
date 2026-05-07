import React, { useEffect, useState } from "react";
import axios from "axios";
import { Button, Card, Empty, Form, Input, Modal, Popconfirm, Select, Space, Spin, Tag, message } from "antd";
import { getAuthHeaders, getJsonAuthHeaders } from "../Utils/auth";
import { ROLE_OPTIONS, ROLE_OPTIONS_ASSIGN } from "../Utils/constants";
import { extractApiError } from "../Utils/apiError";

export default function DeveloperHome() {
  const [usersList, setUsersList] = useState([]);
  const [organizations, setOrganizations] = useState([]);
  const [pendingRoles, setPendingRoles] = useState({});
  const [loading, setLoading] = useState(false);
  const [savingUserId, setSavingUserId] = useState(null);
  const [deletingUserId, setDeletingUserId] = useState(null);

  const [addModalOpen, setAddModalOpen] = useState(false);
  const [addSaving, setAddSaving] = useState(false);
  const [addForm] = Form.useForm();

  useEffect(() => {
    loadUsers();
    loadOrganizations();
  }, []);

  const loadUsers = async () => {
    setLoading(true);
    try {
      const response = await axios.get(`${import.meta.env.VITE_API_BASE_URL}/users`, {
        headers: getAuthHeaders(),
      });
      setUsersList(response.data?.users || []);
      setPendingRoles({});
    } catch {
      setUsersList([]);
      message.error("Não foi possível carregar os usuários.");
    } finally {
      setLoading(false);
    }
  };

  const loadOrganizations = async () => {
    try {
      const response = await axios.get(`${import.meta.env.VITE_API_BASE_URL}/organizations`, {
        headers: getAuthHeaders(),
      });
      setOrganizations(response.data?.organizations || []);
    } catch {
      setOrganizations([]);
    }
  };

  const onRoleChange = (userId, role) => {
    setPendingRoles((prev) => ({ ...prev, [userId]: role }));
  };

  const handleSaveRole = async (user) => {
    const roleToSave = pendingRoles[user.userId] ?? user.role;
    setSavingUserId(user.userId);
    try {
      await axios.put(
        `${import.meta.env.VITE_API_BASE_URL}/user/role`,
        { userId: user.userId, role: roleToSave },
        { headers: getJsonAuthHeaders() }
      );
      message.success("Cargo atualizado com sucesso.");
      await loadUsers();
    } catch {
      message.error("Não foi possível atualizar o cargo.");
    } finally {
      setSavingUserId(null);
    }
  };

  const handleDelete = async (userId) => {
    setDeletingUserId(userId);
    try {
      await axios.delete(`${import.meta.env.VITE_API_BASE_URL}/user/${userId}`, { headers: getAuthHeaders() });
      message.success("Usuário excluído com sucesso.");
      await loadUsers();
    } catch (error) {
      message.error(extractApiError(error, "Não foi possível excluir o usuário."));
    } finally {
      setDeletingUserId(null);
    }
  };

  const handleAddUser = async () => {
    try {
      const values = await addForm.validateFields();
      setAddSaving(true);
      await axios.post(
        `${import.meta.env.VITE_API_BASE_URL}/user/admin`,
        {
          name: values.name.trim(),
          email: values.email.trim(),
          password: values.password,
          role: values.role,
          organizationIds: values.organizationIds ?? [],
        },
        { headers: getJsonAuthHeaders() }
      );
      message.success("Usuário criado com sucesso.");
      addForm.resetFields();
      setAddModalOpen(false);
      await loadUsers();
    } catch (error) {
      if (!error?.errorFields) {
        message.error(extractApiError(error, "Não foi possível criar o usuário."));
      }
    } finally {
      setAddSaving(false);
    }
  };

  const roleColor = (role) => {
    if (role === "Admin") return "red";
    if (role === "Developer") return "purple";
    if (role === "Citizen") return "blue";
    return "default";
  };

  return (
    <div>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 16 }}>
        <div>
          <h2>Gerenciar Cargos</h2>
          <p>Libere ou altere o cargo dos usuários cadastrados no sistema.</p>
        </div>
        <Button type="primary" onClick={() => setAddModalOpen(true)}>
          Adicionar usuário
        </Button>
      </div>

      {loading ? (
        <div className="admin-dashboard-loading">
          <Spin size="large" />
        </div>
      ) : usersList.length === 0 ? (
        <Empty description="Nenhum usuário encontrado." />
      ) : (
        <div className="admin-user-role-list">
          {usersList.map((user) => (
            <Card key={user.userId} size="small" className="admin-user-role-card">
              <div className="admin-user-role-row">
                <div>
                  <strong>{user.name}</strong>
                  <p>{user.email}</p>
                  <Tag color={roleColor(user.role)}>{user.role || "Sem cargo"}</Tag>
                </div>
                <Space>
                  <Select
                    value={pendingRoles[user.userId] ?? user.role ?? undefined}
                    placeholder="Selecione um cargo"
                    options={ROLE_OPTIONS_ASSIGN}
                    onChange={(role) => onRoleChange(user.userId, role)}
                    style={{ minWidth: 150 }}
                  />
                  <Button
                    type="primary"
                    loading={savingUserId === user.userId}
                    onClick={() => handleSaveRole(user)}
                  >
                    Salvar cargo
                  </Button>
                  <Popconfirm
                    title="Excluir usuário"
                    description="Tem certeza que deseja excluir este usuário?"
                    okText="Excluir"
                    cancelText="Cancelar"
                    onConfirm={() => handleDelete(user.userId)}
                  >
                    <Button danger loading={deletingUserId === user.userId}>Excluir</Button>
                  </Popconfirm>
                </Space>
              </div>
            </Card>
          ))}
        </div>
      )}

      <Modal
        title="Adicionar usuário"
        open={addModalOpen}
        onCancel={() => { setAddModalOpen(false); addForm.resetFields(); }}
        onOk={handleAddUser}
        confirmLoading={addSaving}
        okText="Criar"
        cancelText="Cancelar"
      >
        <Form form={addForm} layout="vertical">
          <Form.Item label="Nome" name="name" rules={[{ required: true, message: "Informe o nome." }]}>
            <Input />
          </Form.Item>

          <Form.Item
            label="E-mail"
            name="email"
            rules={[
              { required: true, message: "Informe o e-mail." },
              { type: "email", message: "E-mail inválido." },
            ]}
          >
            <Input />
          </Form.Item>

          <Form.Item
            label="Senha"
            name="password"
            rules={[{ required: true, message: "Informe a senha." }, { min: 6, message: "Mínimo 6 caracteres." }]}
          >
            <Input.Password />
          </Form.Item>

          <Form.Item
            label="Cargo"
            name="role"
            rules={[{ required: true, message: "Selecione um cargo." }]}
          >
            <Select options={ROLE_OPTIONS} placeholder="Selecione" />
          </Form.Item>

          <Form.Item label="Instituições" name="organizationIds">
            <Select
              mode="multiple"
              placeholder="Selecione as instituições (opcional)"
              options={organizations.map((o) => ({ value: o.id, label: o.name }))}
            />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
