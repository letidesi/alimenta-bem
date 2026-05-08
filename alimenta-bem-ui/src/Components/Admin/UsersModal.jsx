import React, { useEffect, useState } from "react";
import axios from "axios";
import { Button, Card, Empty, Form, Input, Modal, Popconfirm, Select, Space, Spin, Tag, message } from "antd";
import { ROLE_OPTIONS_ADMIN } from "../../Utils/constants";
import { extractApiError } from "../../Utils/apiError";

export default function UsersModal({ open, onClose, organizations = [] }) {
  const [usersList, setUsersList] = useState([]);
  const [pendingRoles, setPendingRoles] = useState({});
  const [loading, setLoading] = useState(false);
  const [savingUserId, setSavingUserId] = useState(null);
  const [deletingUserId, setDeletingUserId] = useState(null);

  const [addModalOpen, setAddModalOpen] = useState(false);
  const [addSaving, setAddSaving] = useState(false);
  const [addForm] = Form.useForm();

  useEffect(() => {
    if (open) loadUsers();
  }, [open]);

  const loadUsers = async () => {
    setLoading(true);
    try {
      const response = await axios.get(`${import.meta.env.VITE_API_BASE_URL}/users`);
      setUsersList(response.data?.users || []);
      setPendingRoles({});
    } catch {
      setUsersList([]);
      message.error("Não foi possível carregar os usuários.");
    } finally {
      setLoading(false);
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
      await axios.delete(`${import.meta.env.VITE_API_BASE_URL}/user/${userId}`);
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
      );

      message.success("Usuário criado e vinculado à instituição com sucesso.");
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
    <>
      <Modal
        title="Gerenciar usuários e cargos"
        open={open}
        onCancel={onClose}
        footer={[
          <Button key="add" type="primary" onClick={() => setAddModalOpen(true)}>
            Adicionar usuário
          </Button>,
          <Button key="close" onClick={onClose}>Fechar</Button>,
        ]}
        width={780}
      >
        {loading ? (
          <div className="admin-dashboard-loading"><Spin size="large" /></div>
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
                      placeholder="Sem cargo"
                      options={ROLE_OPTIONS_ADMIN}
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
      </Modal>

      <Modal
        title="Adicionar usuário"
        open={addModalOpen}
        onCancel={() => { setAddModalOpen(false); addForm.resetFields(); }}
        onOk={handleAddUser}
        confirmLoading={addSaving}
        okText="Criar e vincular"
        cancelText="Cancelar"
        width={520}
        styles={{ body: { padding: "24px 32px 8px" } }}
      >
        <Form form={addForm} layout="vertical" requiredMark={false}>
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
            rules={[{ required: true, message: "Informe a senha." }, { min: 12, message: "Mínimo 12 caracteres." }]}
          >
            <Input.Password />
          </Form.Item>

          <Form.Item
            label="Cargo"
            name="role"
            rules={[{ required: true, message: "Selecione um cargo." }]}
          >
            <Select options={ROLE_OPTIONS_ADMIN} placeholder="Selecione" />
          </Form.Item>

          <Form.Item
            label="Instituições"
            name="organizationIds"
            rules={[{ required: true, message: "Selecione ao menos uma instituição." }]}
          >
            <Select
              mode="multiple"
              placeholder="Selecione as instituições"
              options={organizations.map((o) => ({ value: o.id, label: o.name }))}
            />
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
