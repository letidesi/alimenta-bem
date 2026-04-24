import React, { useState } from "react";
import axios from "axios";
import {
  Button, Card, Empty, Form, Input, Modal, Popconfirm, Select, Space, message,
} from "antd";
import { getAuthHeaders, getJsonAuthHeaders } from "../../Utils/auth";
import { ORGANIZATION_TYPE_OPTIONS } from "../../Utils/constants";

export default function OrganizationsModal({ open, onClose, organizations, onRefresh }) {
  const [editingOrganization, setEditingOrganization] = useState(null);
  const [saving, setSaving] = useState(false);
  const [deletingId, setDeletingId] = useState(null);
  const [form] = Form.useForm();

  const openEdit = (organization) => {
    setEditingOrganization(organization);
    form.setFieldsValue({
      id:          organization.id,
      name:        organization.name,
      type:        organization.type,
      description: organization.description,
    });
  };

  const closeEdit = () => {
    setEditingOrganization(null);
    form.resetFields();
  };

  const handleUpdate = async () => {
    if (!editingOrganization) return;
    try {
      const values = await form.validateFields();
      setSaving(true);
      await axios.put(
        `${import.meta.env.VITE_API_BASE_URL}/organization`,
        { id: values.id, name: values.name, type: values.type, description: values.description },
        { headers: getJsonAuthHeaders() }
      );
      message.success("Instituição atualizada com sucesso.");
      closeEdit();
      await onRefresh();
    } catch (error) {
      if (!error?.errorFields) message.error("Não foi possível atualizar a instituição.");
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async (organizationId) => {
    setDeletingId(organizationId);
    try {
      await axios.delete(
        `${import.meta.env.VITE_API_BASE_URL}/organization/${organizationId}`,
        { headers: getAuthHeaders() }
      );
      message.success("Instituição excluída com sucesso.");
      await onRefresh();
    } catch {
      message.error("Não foi possível excluir a instituição.");
    } finally {
      setDeletingId(null);
    }
  };

  return (
    <>
      <Modal
        title="Gerenciar instituições"
        open={open}
        onCancel={onClose}
        footer={[<Button key="close" onClick={onClose}>Fechar</Button>]}
        width={980}
      >
        {organizations.length === 0 ? (
          <Empty description="Nenhuma instituição encontrada." />
        ) : (
          <div className="admin-donor-list">
            {organizations.map((org) => (
              <Card key={org.id} size="small" className="admin-donor-card">
                <div className="admin-donor-row">
                  <div>
                    <strong>{org.name}</strong>
                    <p>Tipo: {org.type || "Não informado"}</p>
                    <p>{org.description || "Sem descrição."}</p>
                  </div>
                  <Space>
                    <Button onClick={() => openEdit(org)}>Editar</Button>
                    <Popconfirm
                      title="Excluir instituição"
                      description="Tem certeza que deseja excluir esta instituição?"
                      okText="Excluir"
                      cancelText="Cancelar"
                      onConfirm={() => handleDelete(org.id)}
                    >
                      <Button danger loading={deletingId === org.id}>Excluir</Button>
                    </Popconfirm>
                  </Space>
                </div>
              </Card>
            ))}
          </div>
        )}
      </Modal>

      <Modal
        title="Editar instituição"
        open={!!editingOrganization}
        onCancel={closeEdit}
        onOk={handleUpdate}
        confirmLoading={saving}
        okText="Salvar"
        cancelText="Cancelar"
      >
        <Form form={form} layout="vertical">
          <Form.Item name="id" hidden><Input /></Form.Item>

          <Form.Item label="Nome" name="name" rules={[{ required: true, message: "Informe o nome." }]}>
            <Input />
          </Form.Item>

          <Form.Item
            label="Tipo"
            name="type"
            rules={[{ required: true, message: "Selecione o tipo da instituição." }]}
          >
            <Select options={ORGANIZATION_TYPE_OPTIONS} />
          </Form.Item>

          <Form.Item label="Descrição" name="description">
            <Input.TextArea rows={3} />
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
