import React, { useEffect, useState } from "react";
import axios from "axios";
import { Button, Card, Empty, Modal, Select, Space, Spin, message } from "antd";
import { getAuthHeaders, getJsonAuthHeaders } from "../../Utils/auth";
import { ROLE_OPTIONS } from "../../Utils/constants";

export default function UsersModal({ open, onClose }) {
  const [usersList, setUsersList] = useState([]);
  const [loading, setLoading] = useState(false);
  const [savingUserId, setSavingUserId] = useState(null);

  useEffect(() => {
    if (open) loadUsers();
  }, [open]);

  const loadUsers = async () => {
    setLoading(true);
    try {
      const response = await axios.get(`${import.meta.env.VITE_API_BASE_URL}/users`, {
        headers: getAuthHeaders(),
      });
      setUsersList(response.data?.users || []);
    } catch {
      setUsersList([]);
      message.error("Não foi possível carregar os usuários.");
    } finally {
      setLoading(false);
    }
  };

  const onRoleChange = (userId, role) => {
    setUsersList((prev) =>
      prev.map((user) => (user.userId === userId ? { ...user, role } : user))
    );
  };

  const handleSaveRole = async (user) => {
    setSavingUserId(user.userId);
    try {
      const response = await axios.put(
        `${import.meta.env.VITE_API_BASE_URL}/user/role`,
        { userId: user.userId, role: user.role },
        { headers: getJsonAuthHeaders() }
      );
      setUsersList((prev) =>
        prev.map((item) =>
          item.userId === response.data.userId ? { ...item, role: response.data.role } : item
        )
      );
      message.success("Cargo atualizado com sucesso.");
    } catch {
      message.error("Não foi possível atualizar o cargo.");
    } finally {
      setSavingUserId(null);
    }
  };

  return (
    <Modal
      title="Gerenciar usuários e cargos"
      open={open}
      onCancel={onClose}
      footer={[
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
                </div>
                <Space>
                  <Select
                    value={user.role || "Citizen"}
                    options={ROLE_OPTIONS}
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
                </Space>
              </div>
            </Card>
          ))}
        </div>
      )}
    </Modal>
  );
}
