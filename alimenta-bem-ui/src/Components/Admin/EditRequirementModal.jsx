import React, { useEffect, useState } from "react";
import axios from "axios";
import { Form, Input, InputNumber, Modal, Select, message } from "antd";
import { getJsonAuthHeaders } from "../../Utils/auth";
import { PRIORITY_OPTIONS } from "../../Utils/constants";

export default function EditRequirementModal({ editingRequirement, organizations, onClose, onSave }) {
  const [form] = Form.useForm();
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (!editingRequirement) return;
    form.setFieldsValue({
      organizationId: editingRequirement.organizationId,
      itemName:       editingRequirement.itemName,
      quantity:       editingRequirement.quantity,
      type:           editingRequirement.type,
    });
  }, [editingRequirement, form]);

  const handleClose = () => {
    form.resetFields();
    onClose();
  };

  const handleSave = async () => {
    try {
      const values = await form.validateFields();
      setSaving(true);

      const response = await axios.put(
        `${import.meta.env.VITE_API_BASE_URL}/organization-requirement`,
        {
          id:             editingRequirement.id,
          organizationId: values.organizationId,
          itemName:       values.itemName,
          quantity:       values.quantity,
          type:           values.type,
        },
        { headers: getJsonAuthHeaders() }
      );

      onSave(response.data);
      message.success("Necessidade atualizada com sucesso.");
      handleClose();
    } catch (error) {
      if (!error?.errorFields) {
        message.error("Não foi possível atualizar a necessidade.");
      }
    } finally {
      setSaving(false);
    }
  };

  return (
    <Modal
      title="Editar necessidade"
      open={!!editingRequirement}
      onCancel={handleClose}
      onOk={handleSave}
      confirmLoading={saving}
      okText="Salvar"
      cancelText="Cancelar"
    >
      <Form form={form} layout="vertical">
        <Form.Item
          label="Instituição"
          name="organizationId"
          rules={[{ required: true, message: "Selecione a instituição." }]}
        >
          <Select
            options={organizations.map((org) => ({ value: org.id, label: org.name }))}
          />
        </Form.Item>

        <Form.Item
          label="Item"
          name="itemName"
          rules={[{ required: true, message: "Informe o item." }]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          label="Quantidade"
          name="quantity"
          rules={[{ required: true, message: "Informe a quantidade." }]}
        >
          <InputNumber min={1} style={{ width: "100%" }} />
        </Form.Item>

        <Form.Item
          label="Prioridade"
          name="type"
          rules={[{ required: true, message: "Selecione a prioridade." }]}
        >
          <Select options={PRIORITY_OPTIONS} />
        </Form.Item>
      </Form>
    </Modal>
  );
}
