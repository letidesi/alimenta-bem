import React, { useEffect, useState } from "react";
import axios from "axios";
import { Button, Card, Empty, Modal, Select, Space, Spin, Tag, message } from "antd";
import { getAuthHeaders, getJsonAuthHeaders } from "../../Utils/auth";
import { UNAVAILABLE_REASON_OPTIONS } from "../../Utils/constants";
import {
  getDonationStatusColor,
  getDonationStatusLabel,
  isDonationFinalized,
} from "../../Utils/donationStatus";

export default function DonationQueueModal({ open, onClose, organizations, onQueueChanged }) {
  const [queue, setQueue] = useState([]);
  const [loading, setLoading] = useState(false);
  const [selectedOrgId, setSelectedOrgId] = useState("");
  const [updatingId, setUpdatingId] = useState(null);
  const [unavailableModalOpen, setUnavailableModalOpen] = useState(false);
  const [pendingDonationId, setPendingDonationId] = useState(null);
  const [selectedUnavailableReason, setSelectedUnavailableReason] = useState("ReceivingInstability");

  useEffect(() => {
    if (!open) {
      setQueue([]);
      return;
    }
    const firstOrgId = organizations[0]?.id || "";
    setSelectedOrgId(firstOrgId);
    loadQueue(firstOrgId);
  }, [open]);

  const loadQueue = async (organizationId) => {
    if (!organizationId) {
      setQueue([]);
      return;
    }
    setLoading(true);
    try {
      const response = await axios.get(
        `${import.meta.env.VITE_API_BASE_URL}/donations/organization/${organizationId}`,
        { headers: getAuthHeaders() }
      );
      setQueue(response.data?.donations || []);
    } catch {
      setQueue([]);
      message.error("Não foi possível carregar a fila de doações.");
    } finally {
      setLoading(false);
    }
  };

  const updateStatus = async (donationId, status, unavailableReason = null) => {
    if (!selectedOrgId) {
      message.warning("Selecione uma instituição para gerenciar a fila de doações.");
      return;
    }
    setUpdatingId(donationId);
    try {
      await axios.put(
        `${import.meta.env.VITE_API_BASE_URL}/donation/status`,
        { donationId, organizationId: selectedOrgId, status, unavailableReason },
        { headers: getJsonAuthHeaders() }
      );
      message.success("Status da doação atualizado com sucesso.");
      await loadQueue(selectedOrgId);
      if (onQueueChanged) {
        await onQueueChanged();
      }
    } catch {
      message.error("Não foi possível atualizar o status da doação.");
    } finally {
      setUpdatingId(null);
    }
  };

  const openUnavailableModal = (donationId) => {
    setPendingDonationId(donationId);
    setSelectedUnavailableReason("ReceivingInstability");
    setUnavailableModalOpen(true);
  };

  const confirmUnavailable = async () => {
    if (!pendingDonationId) return;
    await updateStatus(pendingDonationId, "TemporarilyUnavailable", selectedUnavailableReason);
    setUnavailableModalOpen(false);
    setPendingDonationId(null);
  };

  const handleClose = () => {
    setQueue([]);
    setPendingDonationId(null);
    setUnavailableModalOpen(false);
    onClose();
  };

  return (
    <>
      <Modal
        title="Fila de doações por instituição"
        open={open}
        onCancel={handleClose}
        footer={[<Button key="close" onClick={handleClose}>Fechar</Button>]}
        width={1100}
      >
        <div className="admin-donation-queue-header">
          <Select
            placeholder="Selecione a instituição"
            value={selectedOrgId || undefined}
            style={{ width: "100%" }}
            options={organizations.map((org) => ({ value: org.id, label: org.name }))}
            onChange={async (value) => {
              setSelectedOrgId(value);
              await loadQueue(value);
            }}
          />
        </div>

        {loading ? (
          <div className="admin-dashboard-loading"><Spin size="large" /></div>
        ) : queue.length === 0 ? (
          <Empty description="Nenhuma doação encontrada para esta instituição." />
        ) : (
          <div className="admin-donation-queue-list">
            {queue.map((donation) => {
              const finalized = isDonationFinalized(donation.status);
              return (
                <Card key={donation.id} size="small" className="admin-donation-queue-card">
                  <div className="admin-donation-queue-row">
                    <div>
                      <strong>{donation.itemName}</strong>
                      <p>Doador: {donation.donorName}</p>
                      <p>Quantidade: {donation.amountDonated}</p>
                      <Tag color={getDonationStatusColor(donation.status)}>
                        {getDonationStatusLabel(donation.status)}
                      </Tag>
                      {donation.unavailableMessage && (
                        <p className="donation-unavailable-message">{donation.unavailableMessage}</p>
                      )}
                    </div>
                    <Space wrap>
                      <Button
                        onClick={() => updateStatus(donation.id, "InReview")}
                        loading={updatingId === donation.id}
                        disabled={finalized}
                      >
                        Em análise
                      </Button>
                      <Button
                        onClick={() => updateStatus(donation.id, "ReadyForDelivery")}
                        loading={updatingId === donation.id}
                        disabled={finalized}
                      >
                        Aguardando entrega
                      </Button>
                      <Button
                        type="primary"
                        onClick={() => updateStatus(donation.id, "Received")}
                        loading={updatingId === donation.id}
                        disabled={finalized}
                      >
                        Marcar recebida
                      </Button>
                      <Button
                        danger
                        onClick={() => openUnavailableModal(donation.id)}
                        loading={updatingId === donation.id}
                        disabled={finalized}
                      >
                        Indisponível no momento
                      </Button>
                    </Space>
                  </div>
                </Card>
              );
            })}
          </div>
        )}
      </Modal>

      <Modal
        title="Motivo do redirecionamento da doação"
        open={unavailableModalOpen}
        onCancel={() => {
          setUnavailableModalOpen(false);
          setPendingDonationId(null);
        }}
        onOk={confirmUnavailable}
        okText="Confirmar"
        cancelText="Cancelar"
      >
        <p>Selecione um motivo para o cidadão receber uma justificativa clara.</p>
        <Select
          style={{ width: "100%" }}
          value={selectedUnavailableReason}
          options={UNAVAILABLE_REASON_OPTIONS}
          onChange={setSelectedUnavailableReason}
        />
      </Modal>
    </>
  );
}
