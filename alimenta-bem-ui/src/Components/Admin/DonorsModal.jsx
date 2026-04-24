import React, { useEffect, useState } from "react";
import axios from "axios";
import {
  Button, Card, Empty, Form, Input, Modal, Popconfirm, Select, Space, Spin, message,
} from "antd";
import { getAuthHeaders, getJsonAuthHeaders } from "../../Utils/auth";
import { GENDER_OPTIONS, SKIN_COLOR_OPTIONS } from "../../Utils/constants";

export default function DonorsModal({ open, onClose }) {
  const [donors, setDonors] = useState([]);
  const [loading, setLoading] = useState(false);
  const [editingDonor, setEditingDonor] = useState(null);
  const [saving, setSaving] = useState(false);
  const [deletingId, setDeletingId] = useState(null);
  const [form] = Form.useForm();

  useEffect(() => {
    if (open) loadDonors();
  }, [open]);

  const loadDonors = async () => {
    setLoading(true);
    try {
      const response = await axios.get(
        `${import.meta.env.VITE_API_BASE_URL}/natural-persons/admin`,
        { headers: getAuthHeaders() }
      );
      setDonors(response.data?.naturalPersons || []);
    } catch {
      setDonors([]);
      message.error("Não foi possível carregar os doadores.");
    } finally {
      setLoading(false);
    }
  };

  const openEdit = (donor) => {
    setEditingDonor(donor);
    form.setFieldsValue({
      userId:       donor.userId,
      email:        donor.emailUser,
      name:         donor.name,
      socialName:   donor.socialName,
      age:          donor.age,
      birthdayDate: donor.birthdayDate,
      gender:       donor.gender,
      skinColor:    donor.skinColor,
      isPcd:        donor.isPcd,
    });
  };

  const closeEdit = () => {
    setEditingDonor(null);
    form.resetFields();
  };

  const handleUpdate = async () => {
    if (!editingDonor) return;
    try {
      const values = await form.validateFields();
      setSaving(true);
      await axios.put(
        `${import.meta.env.VITE_API_BASE_URL}/natural-person/admin`,
        {
          userId:       values.userId,
          email:        values.email,
          name:         values.name,
          socialName:   values.socialName,
          age:          values.age,
          birthdayDate: values.birthdayDate,
          gender:       values.gender,
          skinColor:    values.skinColor,
          isPcd:        values.isPcd,
        },
        { headers: getJsonAuthHeaders() }
      );
      message.success("Doador atualizado com sucesso.");
      closeEdit();
      await loadDonors();
    } catch (error) {
      if (!error?.errorFields) message.error("Não foi possível atualizar o doador.");
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async (userId) => {
    setDeletingId(userId);
    try {
      await axios.delete(
        `${import.meta.env.VITE_API_BASE_URL}/natural-person/admin/${userId}`,
        { headers: getAuthHeaders() }
      );
      message.success("Doador excluído com sucesso.");
      await loadDonors();
    } catch {
      message.error("Não foi possível excluir o doador.");
    } finally {
      setDeletingId(null);
    }
  };

  return (
    <>
      <Modal
        title="Gerenciar doadores"
        open={open}
        onCancel={onClose}
        footer={[<Button key="close" onClick={onClose}>Fechar</Button>]}
        width={980}
      >
        {loading ? (
          <div className="admin-dashboard-loading"><Spin size="large" /></div>
        ) : donors.length === 0 ? (
          <Empty description="Nenhum doador encontrado." />
        ) : (
          <div className="admin-donor-list">
            {donors.map((donor) => (
              <Card key={donor.id} size="small" className="admin-donor-card">
                <div className="admin-donor-row">
                  <div>
                    <strong>{donor.name}</strong>
                    <p>{donor.emailUser}</p>
                    <p>Doações realizadas: <b>{donor.donationCount}</b></p>
                  </div>
                  <Space>
                    <Button onClick={() => openEdit(donor)}>Editar</Button>
                    <Popconfirm
                      title="Excluir doador"
                      description="Tem certeza que deseja excluir este doador?"
                      okText="Excluir"
                      cancelText="Cancelar"
                      onConfirm={() => handleDelete(donor.userId)}
                    >
                      <Button danger loading={deletingId === donor.userId}>Excluir</Button>
                    </Popconfirm>
                  </Space>
                </div>
              </Card>
            ))}
          </div>
        )}
      </Modal>

      <Modal
        title="Editar doador"
        open={!!editingDonor}
        onCancel={closeEdit}
        onOk={handleUpdate}
        confirmLoading={saving}
        okText="Salvar"
        cancelText="Cancelar"
      >
        <Form form={form} layout="vertical">
          <Form.Item name="userId" hidden><Input /></Form.Item>

          <Form.Item
            label="E-mail"
            name="email"
            rules={[
              { required: true, message: "Informe o e-mail." },
              { type: "email", message: "Informe um e-mail válido." },
            ]}
          >
            <Input type="email" />
          </Form.Item>

          <Form.Item label="Nome" name="name" rules={[{ required: true, message: "Informe o nome." }]}>
            <Input />
          </Form.Item>

          <Form.Item label="Nome social" name="socialName">
            <Input />
          </Form.Item>

          <Form.Item label="Idade" name="age" rules={[{ required: true, message: "Informe a idade." }]}>
            <Input />
          </Form.Item>

          <Form.Item
            label="Data de nascimento"
            name="birthdayDate"
            rules={[{ required: true, message: "Informe a data de nascimento." }]}
          >
            <Input type="date" />
          </Form.Item>

          <Form.Item label="Gênero" name="gender">
            <Select options={GENDER_OPTIONS} allowClear />
          </Form.Item>

          <Form.Item label="Raça" name="skinColor">
            <Select options={SKIN_COLOR_OPTIONS} allowClear />
          </Form.Item>

          <Form.Item label="É PCD" name="isPcd">
            <Select options={[{ value: true, label: "Sim" }, { value: false, label: "Não" }]} />
          </Form.Item>
        </Form>
      </Modal>
    </>
  );
}
