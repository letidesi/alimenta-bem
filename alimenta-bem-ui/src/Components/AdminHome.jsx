import React, { useEffect, useMemo, useState } from "react";
import axios from "axios";
import {
  Button,
  Card,
  Col,
  Empty,
  Form,
  Input,
  InputNumber,
  Modal,
  Pagination,
  Popconfirm,
  Row,
  Select,
  Space,
  Spin,
  Statistic,
  Tag,
  message,
} from "antd";
import { Link } from "react-router-dom";
import "../Css/Style.css";

export default function AdminHome() {
  const [form] = Form.useForm();
  const [donorForm] = Form.useForm();
  const [organizationForm] = Form.useForm();
  const [loading, setLoading] = useState(true);
  const [organizations, setOrganizations] = useState([]);
  const [donors, setDonors] = useState([]);
  const [adminDonors, setAdminDonors] = useState([]);
  const [requirementsByOrganization, setRequirementsByOrganization] = useState([]);
  const [usersList, setUsersList] = useState([]);
  const [editingRequirement, setEditingRequirement] = useState(null);
  const [usersModalOpen, setUsersModalOpen] = useState(false);
  const [donorsModalOpen, setDonorsModalOpen] = useState(false);
  const [organizationsModalOpen, setOrganizationsModalOpen] = useState(false);
  const [donationsModalOpen, setDonationsModalOpen] = useState(false);
  const [unavailableModalOpen, setUnavailableModalOpen] = useState(false);
  const [editDonorModalOpen, setEditDonorModalOpen] = useState(false);
  const [editOrganizationModalOpen, setEditOrganizationModalOpen] = useState(false);
  const [savingRequirement, setSavingRequirement] = useState(false);
  const [loadingUsers, setLoadingUsers] = useState(false);
  const [loadingAdminDonors, setLoadingAdminDonors] = useState(false);
  const [loadingDonationQueue, setLoadingDonationQueue] = useState(false);
  const [savingDonor, setSavingDonor] = useState(false);
  const [savingOrganization, setSavingOrganization] = useState(false);
  const [updatingDonationId, setUpdatingDonationId] = useState(null);
  const [deletingOrganizationId, setDeletingOrganizationId] = useState(null);
  const [deletingDonorId, setDeletingDonorId] = useState(null);
  const [savingUserRoleId, setSavingUserRoleId] = useState(null);
  const [editingDonor, setEditingDonor] = useState(null);
  const [editingOrganization, setEditingOrganization] = useState(null);
  const [donationsQueue, setDonationsQueue] = useState([]);
  const [selectedDonationOrganizationId, setSelectedDonationOrganizationId] = useState("");
  const [pendingUnavailableDonationId, setPendingUnavailableDonationId] = useState(null);
  const [selectedUnavailableReason, setSelectedUnavailableReason] = useState("ReceivingInstability");
  const [institutionSearch, setInstitutionSearch] = useState("");
  const [itemSearch, setItemSearch] = useState("");
  const [priorityFilter, setPriorityFilter] = useState("all");
  const [currentPage, setCurrentPage] = useState(1);
  const PAGE_SIZE = 20;
  const token = localStorage.getItem("accessToken");
  const roleOptions = [
    { value: "Admin", label: "Admin" },
    { value: "Developer", label: "Developer" },
    { value: "Citizen", label: "Citizen" },
  ];
  const unavailableReasonOptions = [
    { value: "ReceivingInstability", label: "Instabilidade temporária de recebimento" },
    { value: "NeedAlreadyMet", label: "Necessidade já atendida" },
    { value: "OfferNeedsAdjustment", label: "Oferta precisa de ajuste" },
  ];
  const organizationTypeOptions = [
    { value: "Igreja", label: "Igreja" },
    { value: "ONG", label: "ONG" },
    { value: "Escola", label: "Escola" },
  ];

  const loadDashboard = async () => {
    setLoading(true);

    try {
      const [organizationResponse, donorResponse] = await Promise.all([
        axios.get(`${import.meta.env.VITE_API_BASE_URL}/organizations`),
        axios.get(`${import.meta.env.VITE_API_BASE_URL}/natural-persons`),
      ]);

      const organizationList = organizationResponse.data?.organizations || [];
      const donorList = donorResponse.data?.naturalPersons || [];

      setOrganizations(organizationList);
      setDonors(donorList);

      const requirementsResponses = await Promise.all(
        organizationList.map(async (organization) => {
          try {
            const response = await axios.get(
              `${import.meta.env.VITE_API_BASE_URL}/organization-requirements/${organization.id}`
            );

            return {
              ...organization,
              requirements: response.data?.requirements || [],
            };
          } catch {
            return {
              ...organization,
              requirements: [],
            };
          }
        })
      );

      setRequirementsByOrganization(requirementsResponses);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadDashboard();
  }, []);

  const filteredOrganizations = useMemo(() => {
    const instLower = institutionSearch.trim().toLowerCase();
    const itemLower = itemSearch.trim().toLowerCase();

    return requirementsByOrganization
      .filter((org) => !instLower || org.name.toLowerCase().includes(instLower))
      .map((org) => ({
        ...org,
        requirements: org.requirements.filter((req) => {
          const matchItem = !itemLower || req.itemName.toLowerCase().includes(itemLower);
          const matchPriority = priorityFilter === "all" || req.type === priorityFilter;
          return matchItem && matchPriority;
        }),
      }))
      .filter((org) => {
        if (itemLower || priorityFilter !== "all") return org.requirements.length > 0;
        return true;
      });
  }, [requirementsByOrganization, institutionSearch, itemSearch, priorityFilter]);

  const paginatedOrganizations = useMemo(() => {
    const start = (currentPage - 1) * PAGE_SIZE;
    return filteredOrganizations.slice(start, start + PAGE_SIZE);
  }, [filteredOrganizations, currentPage]);

  const totalRequirements = useMemo(
    () => requirementsByOrganization.reduce((sum, item) => sum + item.requirements.length, 0),
    [requirementsByOrganization]
  );

  const totalRequestedQuantity = useMemo(
    () =>
      requirementsByOrganization.reduce(
        (sum, item) => sum + item.requirements.reduce((inner, req) => inner + (req.quantity || 0), 0),
        0
      ),
    [requirementsByOrganization]
  );

  const priorityColor = (priority) => {
    if (priority === "Alta") return "red";
    if (priority === "Media") return "gold";
    if (priority === "Baixa") return "blue";
    return "default";
  };

  const openEditModal = (organization, requirement) => {
    const nextRequirement = {
      ...requirement,
      organizationId: organization.id,
    };

    setEditingRequirement(nextRequirement);
    form.setFieldsValue({
      organizationId: organization.id,
      itemName: requirement.itemName,
      quantity: requirement.quantity,
      type: requirement.type,
    });
  };

  const closeEditModal = () => {
    setEditingRequirement(null);
    form.resetFields();
  };

  const updateRequirementState = (updatedRequirement) => {
    setRequirementsByOrganization((prev) => {
      const cleaned = prev.map((organization) => ({
        ...organization,
        requirements: organization.requirements.filter((req) => req.id !== updatedRequirement.id),
      }));

      return cleaned.map((organization) => {
        if (organization.id !== updatedRequirement.organizationId) {
          return organization;
        }

        return {
          ...organization,
          requirements: [
            ...organization.requirements,
            {
              id: updatedRequirement.id,
              itemName: updatedRequirement.itemName,
              quantity: updatedRequirement.quantity,
              type: updatedRequirement.type,
            },
          ].sort((a, b) => a.itemName.localeCompare(b.itemName)),
        };
      });
    });
  };

  const deleteRequirementState = (requirementId) => {
    setRequirementsByOrganization((prev) =>
      prev.map((organization) => ({
        ...organization,
        requirements: organization.requirements.filter((req) => req.id !== requirementId),
      }))
    );
  };

  const handleEditRequirement = async () => {
    if (!editingRequirement) return;

    try {
      const values = await form.validateFields();
      setSavingRequirement(true);

      const response = await axios.put(
        `${import.meta.env.VITE_API_BASE_URL}/organization-requirement`,
        {
          id: editingRequirement.id,
          organizationId: values.organizationId,
          itemName: values.itemName,
          quantity: values.quantity,
          type: values.type,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );

      updateRequirementState(response.data);
      message.success("Necessidade atualizada com sucesso.");
      closeEditModal();
    } catch (error) {
      if (!error?.errorFields) {
        message.error("Não foi possível atualizar a necessidade.");
      }
    } finally {
      setSavingRequirement(false);
    }
  };

  const handleDeleteRequirement = async (requirementId) => {
    try {
      await axios.delete(
        `${import.meta.env.VITE_API_BASE_URL}/organization-requirement/${requirementId}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      deleteRequirementState(requirementId);
      message.success("Necessidade removida com sucesso.");
    } catch {
      message.error("Não foi possível remover a necessidade.");
    }
  };

  const loadUsers = async () => {
    setLoadingUsers(true);

    try {
      const response = await axios.get(`${import.meta.env.VITE_API_BASE_URL}/users`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      setUsersList(response.data?.users || []);
    } catch {
      setUsersList([]);
      message.error("Não foi possível carregar os usuários.");
    } finally {
      setLoadingUsers(false);
    }
  };

  const openUsersModal = async () => {
    setUsersModalOpen(true);
    await loadUsers();
  };

  const closeUsersModal = () => {
    setUsersModalOpen(false);
  };

  const onRoleSelectChange = (userId, role) => {
    setUsersList((prev) =>
      prev.map((user) => (user.userId === userId ? { ...user, role } : user))
    );
  };

  const handleSaveUserRole = async (user) => {
    setSavingUserRoleId(user.userId);

    try {
      const response = await axios.put(
        `${import.meta.env.VITE_API_BASE_URL}/user/role`,
        {
          userId: user.userId,
          role: user.role,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );

      setUsersList((prev) =>
        prev.map((item) =>
          item.userId === response.data.userId
            ? { ...item, role: response.data.role }
            : item
        )
      );

      message.success("Cargo atualizado com sucesso.");
    } catch {
      message.error("Não foi possível atualizar o cargo.");
    } finally {
      setSavingUserRoleId(null);
    }
  };

  const loadAdminDonors = async () => {
    setLoadingAdminDonors(true);

    try {
      const response = await axios.get(`${import.meta.env.VITE_API_BASE_URL}/natural-persons/admin`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      const donorList = response.data?.naturalPersons || [];
      setAdminDonors(donorList);
      setDonors(donorList);
    } catch {
      setAdminDonors([]);
      message.error("Não foi possível carregar os doadores.");
    } finally {
      setLoadingAdminDonors(false);
    }
  };

  const openDonorsModal = async () => {
    setDonorsModalOpen(true);
    await loadAdminDonors();
  };

  const closeDonorsModal = () => {
    setDonorsModalOpen(false);
  };

  const openEditDonorModal = (donor) => {
    setEditingDonor(donor);
    donorForm.setFieldsValue({
      userId: donor.userId,
      email: donor.emailUser,
      name: donor.name,
      socialName: donor.socialName,
      age: donor.age,
      birthdayDate: donor.birthdayDate,
      gender: donor.gender,
      skinColor: donor.skinColor,
      isPcd: donor.isPcd,
    });
    setEditDonorModalOpen(true);
  };

  const closeEditDonorModal = () => {
    setEditDonorModalOpen(false);
    setEditingDonor(null);
    donorForm.resetFields();
  };

  const handleUpdateDonor = async () => {
    if (!editingDonor) return;

    try {
      const values = await donorForm.validateFields();
      setSavingDonor(true);

      await axios.put(
        `${import.meta.env.VITE_API_BASE_URL}/natural-person/admin`,
        {
          userId: values.userId,
          email: values.email,
          name: values.name,
          socialName: values.socialName,
          age: values.age,
          birthdayDate: values.birthdayDate,
          gender: values.gender,
          skinColor: values.skinColor,
          isPcd: values.isPcd,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );

      message.success("Doador atualizado com sucesso.");
      closeEditDonorModal();
      await loadAdminDonors();
    } catch (error) {
      if (!error?.errorFields) {
        message.error("Não foi possível atualizar o doador.");
      }
    } finally {
      setSavingDonor(false);
    }
  };

  const handleDeleteDonor = async (userId) => {
    setDeletingDonorId(userId);

    try {
      await axios.delete(`${import.meta.env.VITE_API_BASE_URL}/natural-person/admin/${userId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      message.success("Doador excluído com sucesso.");
      await loadAdminDonors();
    } catch {
      message.error("Não foi possível excluir o doador.");
    } finally {
      setDeletingDonorId(null);
    }
  };

  const openOrganizationsModal = () => {
    setOrganizationsModalOpen(true);
  };

  const closeOrganizationsModal = () => {
    setOrganizationsModalOpen(false);
  };

  const openEditOrganizationModal = (organization) => {
    setEditingOrganization(organization);
    organizationForm.setFieldsValue({
      id: organization.id,
      name: organization.name,
      type: organization.type,
      description: organization.description,
    });
    setEditOrganizationModalOpen(true);
  };

  const closeEditOrganizationModal = () => {
    setEditOrganizationModalOpen(false);
    setEditingOrganization(null);
    organizationForm.resetFields();
  };

  const handleUpdateOrganization = async () => {
    if (!editingOrganization) return;

    try {
      const values = await organizationForm.validateFields();
      setSavingOrganization(true);

      await axios.put(
        `${import.meta.env.VITE_API_BASE_URL}/organization`,
        {
          id: values.id,
          name: values.name,
          type: values.type,
          description: values.description,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );

      message.success("Instituição atualizada com sucesso.");
      closeEditOrganizationModal();
      await loadDashboard();
    } catch (error) {
      if (!error?.errorFields) {
        message.error("Não foi possível atualizar a instituição.");
      }
    } finally {
      setSavingOrganization(false);
    }
  };

  const handleDeleteOrganization = async (organizationId) => {
    setDeletingOrganizationId(organizationId);

    try {
      await axios.delete(`${import.meta.env.VITE_API_BASE_URL}/organization/${organizationId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      message.success("Instituição excluída com sucesso.");
      await loadDashboard();
    } catch {
      message.error("Não foi possível excluir a instituição.");
    } finally {
      setDeletingOrganizationId(null);
    }
  };

  const loadDonationQueue = async (organizationId) => {
    if (!organizationId) {
      setDonationsQueue([]);
      return;
    }

    setLoadingDonationQueue(true);

    try {
      const response = await axios.get(
        `${import.meta.env.VITE_API_BASE_URL}/donations/organization/${organizationId}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      setDonationsQueue(response.data?.donations || []);
    } catch {
      setDonationsQueue([]);
      message.error("Não foi possível carregar a fila de doações.");
    } finally {
      setLoadingDonationQueue(false);
    }
  };

  const openDonationsModal = async () => {
    const firstOrganizationId = organizations[0]?.id || "";
    setSelectedDonationOrganizationId(firstOrganizationId);
    setDonationsModalOpen(true);
    await loadDonationQueue(firstOrganizationId);
  };

  const closeDonationsModal = () => {
    setDonationsModalOpen(false);
    setDonationsQueue([]);
    setPendingUnavailableDonationId(null);
    setUnavailableModalOpen(false);
  };

  const updateDonationStatus = async (donationId, status, unavailableReason = null) => {
    if (!selectedDonationOrganizationId) {
      message.warning("Selecione uma instituição para gerenciar a fila de doações.");
      return;
    }

    setUpdatingDonationId(donationId);

    try {
      await axios.put(
        `${import.meta.env.VITE_API_BASE_URL}/donation/status`,
        {
          donationId,
          organizationId: selectedDonationOrganizationId,
          status,
          unavailableReason,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );

      message.success("Status da doação atualizado com sucesso.");
      await loadDonationQueue(selectedDonationOrganizationId);
    } catch {
      message.error("Não foi possível atualizar o status da doação.");
    } finally {
      setUpdatingDonationId(null);
    }
  };

  const openUnavailableReasonModal = (donationId) => {
    setPendingUnavailableDonationId(donationId);
    setSelectedUnavailableReason("ReceivingInstability");
    setUnavailableModalOpen(true);
  };

  const handleConfirmUnavailable = async () => {
    if (!pendingUnavailableDonationId) return;

    await updateDonationStatus(
      pendingUnavailableDonationId,
      "TemporarilyUnavailable",
      selectedUnavailableReason
    );

    setUnavailableModalOpen(false);
    setPendingUnavailableDonationId(null);
  };

  const donationStatusLabel = (status) => {
    if (status === "Submitted" || status === "Sent" || status === "Enviada") return "Enviada";
    if (status === "InReview" || status === "UnderReview" || status === "EmAnalise") return "Em análise";
    if (status === "ReadyForDelivery" || status === "AwaitingDelivery" || status === "AguardandoEntrega") return "Aguardando entrega";
    if (status === "Received" || status === "Recebida") return "Recebida";
    if (status === "TemporarilyUnavailable" || status === "UnavailableAtTheMoment" || status === "IndisponivelNoMomento") return "Indisponível no momento";
    return status;
  };

  const donationStatusColor = (status) => {
    if (status === "Received" || status === "Recebida") return "green";
    if (status === "ReadyForDelivery" || status === "AwaitingDelivery" || status === "AguardandoEntrega") return "blue";
    if (status === "InReview" || status === "UnderReview" || status === "EmAnalise") return "gold";
    if (status === "TemporarilyUnavailable" || status === "UnavailableAtTheMoment" || status === "IndisponivelNoMomento") return "volcano";
    return "default";
  };

  return (
    <section className="admin-dashboard">
      <div className="home-hero">
        <h1>Painel do Administrador</h1>
        <p>
          Gerencie instituições, acompanhe necessidades cadastradas e visualize
          rapidamente o volume de doadores e itens solicitados.
        </p>
      </div>

      {loading ? (
        <div className="admin-dashboard-loading">
          <Spin size="large" />
        </div>
      ) : (
        <>
          <Row gutter={[16, 16]} className="admin-stats-row">
            <Col xs={24} md={8}>
              <Card className="admin-stat-card">
                <Statistic title="Doadores cadastrados" value={donors.length} />
              </Card>
            </Col>
            <Col xs={24} md={8}>
              <Card className="admin-stat-card">
                <Statistic title="Instituições cadastradas" value={organizations.length} />
              </Card>
            </Col>
            <Col xs={24} md={8}>
              <Card className="admin-stat-card">
                <Statistic title="Itens solicitados" value={totalRequestedQuantity} />
              </Card>
            </Col>
          </Row>

          <Card className="admin-overview-card">
            <div className="admin-overview-header">
              <div>
                <h2>Necessidades publicadas</h2>
                <p>{totalRequirements} necessidades ativas distribuídas pelas instituições.</p>
              </div>
              <div className="admin-overview-actions">
                <Button onClick={openDonorsModal}>Gerenciar doadores</Button>
                <Button onClick={openUsersModal}>Gerenciar usuários</Button>
                <Button onClick={openOrganizationsModal}>Gerenciar instituições</Button>
                <Button onClick={openDonationsModal}>Gerenciar doações</Button>
                <Link to="/admin/create-organization">
                  <Button>Cadastrar instituição</Button>
                </Link>
                <Link to="/admin/organization-req">
                  <Button type="primary">Nova necessidade</Button>
                </Link>
              </div>
            </div>

            {requirementsByOrganization.length === 0 ? (
              <Empty description="Nenhuma instituição cadastrada ainda." />
            ) : (
              <>
                <div className="admin-filters">
                  <Input.Search
                    placeholder="Buscar instituição pelo nome..."
                    allowClear
                    value={institutionSearch}
                    onChange={(e) => {
                      setInstitutionSearch(e.target.value);
                      setCurrentPage(1);
                    }}
                    className="admin-filter-input"
                  />
                  <Input.Search
                    placeholder="Buscar item necessário..."
                    allowClear
                    value={itemSearch}
                    onChange={(e) => {
                      setItemSearch(e.target.value);
                      setCurrentPage(1);
                    }}
                    className="admin-filter-input"
                  />
                  <Select
                    value={priorityFilter}
                    onChange={(value) => {
                      setPriorityFilter(value);
                      setCurrentPage(1);
                    }}
                    className="admin-filter-select"
                    options={[
                      { value: "all", label: "Todas as prioridades" },
                      { value: "Alta", label: "Alta" },
                      { value: "Media", label: "Média" },
                      { value: "Baixa", label: "Baixa" },
                    ]}
                  />
                </div>

                {filteredOrganizations.length === 0 ? (
                  <Empty description="Nenhuma instituição encontrada para os filtros aplicados." />
                ) : (
                  <>
                    <p className="admin-filter-count">
                      Exibindo {paginatedOrganizations.length} de {filteredOrganizations.length} instituição(ões)
                    </p>
                    <div className="organization-need-grid">
                      {paginatedOrganizations.map((organization) => (
                        <Card
                          key={organization.id}
                          className="organization-need-card"
                          title={organization.name}
                        >
                          {organization.requirements.length === 0 ? (
                            <Empty
                              image={Empty.PRESENTED_IMAGE_SIMPLE}
                              description="Sem necessidades cadastradas"
                            />
                          ) : (
                            <ul className="organization-need-list">
                              {organization.requirements.map((requirement) => (
                                <li key={requirement.id} className="organization-need-item">
                                  <div className="organization-need-item-header">
                                    <strong>{requirement.itemName}</strong>
                                    <Tag color={priorityColor(requirement.type)}>{requirement.type}</Tag>
                                  </div>

                                  <div className="organization-need-item-body">
                                    <span>Quantidade solicitada</span>
                                    <b>{requirement.quantity}</b>
                                  </div>

                                  <div className="organization-need-item-actions">
                                    <Button
                                      type="text"
                                      size="small"
                                      onClick={() => openEditModal(organization, requirement)}
                                    >
                                      Editar
                                    </Button>
                                    <Popconfirm
                                      title="Remover necessidade"
                                      description="Tem certeza que deseja excluir esta necessidade?"
                                      okText="Excluir"
                                      cancelText="Cancelar"
                                      onConfirm={() => handleDeleteRequirement(requirement.id)}
                                    >
                                      <Button type="text" size="small" danger>
                                        Excluir
                                      </Button>
                                    </Popconfirm>
                                  </div>
                                </li>
                              ))}
                            </ul>
                          )}
                        </Card>
                      ))}
                    </div>

                    {filteredOrganizations.length > PAGE_SIZE && (
                      <div className="admin-pagination">
                        <Pagination
                          current={currentPage}
                          pageSize={PAGE_SIZE}
                          total={filteredOrganizations.length}
                          onChange={(page) => {
                            setCurrentPage(page);
                            window.scrollTo({ top: 0, behavior: "smooth" });
                          }}
                          showSizeChanger={false}
                          showTotal={(total) => `${total} instituições`}
                        />
                      </div>
                    )}
                  </>
                )}
              </>
            )}
          </Card>
        </>
      )}

      <Modal
        title="Editar necessidade"
        open={!!editingRequirement}
        onCancel={closeEditModal}
        onOk={handleEditRequirement}
        confirmLoading={savingRequirement}
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
              options={organizations.map((organization) => ({
                value: organization.id,
                label: organization.name,
              }))}
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
            <Select
              options={[
                { value: "Alta", label: "Alta" },
                { value: "Media", label: "Média" },
                { value: "Baixa", label: "Baixa" },
              ]}
            />
          </Form.Item>
        </Form>
      </Modal>

      <Modal
        title="Gerenciar instituições"
        open={organizationsModalOpen}
        onCancel={closeOrganizationsModal}
        footer={[
          <Button key="close-organizations-modal" onClick={closeOrganizationsModal}>
            Fechar
          </Button>,
        ]}
        width={980}
      >
        {organizations.length === 0 ? (
          <Empty description="Nenhuma instituição encontrada." />
        ) : (
          <div className="admin-donor-list">
            {organizations.map((organization) => (
              <Card key={organization.id} size="small" className="admin-donor-card">
                <div className="admin-donor-row">
                  <div>
                    <strong>{organization.name}</strong>
                    <p>Tipo: {organization.type || "Não informado"}</p>
                    <p>{organization.description || "Sem descrição."}</p>
                  </div>

                  <Space>
                    <Button onClick={() => openEditOrganizationModal(organization)}>Editar</Button>
                    <Popconfirm
                      title="Excluir instituição"
                      description="Tem certeza que deseja excluir esta instituição?"
                      okText="Excluir"
                      cancelText="Cancelar"
                      onConfirm={() => handleDeleteOrganization(organization.id)}
                    >
                      <Button danger loading={deletingOrganizationId === organization.id}>
                        Excluir
                      </Button>
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
        open={editOrganizationModalOpen}
        onCancel={closeEditOrganizationModal}
        onOk={handleUpdateOrganization}
        confirmLoading={savingOrganization}
        okText="Salvar"
        cancelText="Cancelar"
      >
        <Form form={organizationForm} layout="vertical">
          <Form.Item name="id" hidden>
            <Input />
          </Form.Item>

          <Form.Item label="Nome" name="name" rules={[{ required: true, message: "Informe o nome." }]}>
            <Input />
          </Form.Item>

          <Form.Item
            label="Tipo"
            name="type"
            rules={[{ required: true, message: "Selecione o tipo da instituição." }]}
          >
            <Select options={organizationTypeOptions} />
          </Form.Item>

          <Form.Item label="Descrição" name="description">
            <Input.TextArea rows={3} />
          </Form.Item>
        </Form>
      </Modal>

      <Modal
        title="Fila de doações por instituição"
        open={donationsModalOpen}
        onCancel={closeDonationsModal}
        footer={[
          <Button key="close-donations-modal" onClick={closeDonationsModal}>
            Fechar
          </Button>,
        ]}
        width={1100}
      >
        <div className="admin-donation-queue-header">
          <Select
            placeholder="Selecione a instituição"
            value={selectedDonationOrganizationId || undefined}
            style={{ width: "100%" }}
            options={organizations.map((organization) => ({
              value: organization.id,
              label: organization.name,
            }))}
            onChange={async (value) => {
              setSelectedDonationOrganizationId(value);
              await loadDonationQueue(value);
            }}
          />
        </div>

        {loadingDonationQueue ? (
          <div className="admin-dashboard-loading">
            <Spin size="large" />
          </div>
        ) : donationsQueue.length === 0 ? (
          <Empty description="Nenhuma doação encontrada para esta instituição." />
        ) : (
          <div className="admin-donation-queue-list">
            {donationsQueue.map((donation) => (
              <Card key={donation.id} size="small" className="admin-donation-queue-card">
                <div className="admin-donation-queue-row">
                  <div>
                    <strong>{donation.itemName}</strong>
                    <p>Doador: {donation.donorName}</p>
                    <p>Quantidade: {donation.amountDonated}</p>
                    <Tag color={donationStatusColor(donation.status)}>
                      {donationStatusLabel(donation.status)}
                    </Tag>
                    {donation.unavailableMessage && (
                      <p className="donation-unavailable-message">{donation.unavailableMessage}</p>
                    )}
                  </div>

                  <Space wrap>
                    <Button
                      onClick={() => updateDonationStatus(donation.id, "InReview")}
                      loading={updatingDonationId === donation.id}
                      disabled={
                        donation.status === "Received" ||
                        donation.status === "Recebida" ||
                        donation.status === "TemporarilyUnavailable" ||
                        donation.status === "UnavailableAtTheMoment" ||
                        donation.status === "IndisponivelNoMomento"
                      }
                    >
                      Em análise
                    </Button>
                    <Button
                      onClick={() => updateDonationStatus(donation.id, "ReadyForDelivery")}
                      loading={updatingDonationId === donation.id}
                      disabled={
                        donation.status === "Received" ||
                        donation.status === "Recebida" ||
                        donation.status === "TemporarilyUnavailable" ||
                        donation.status === "UnavailableAtTheMoment" ||
                        donation.status === "IndisponivelNoMomento"
                      }
                    >
                      Aguardando entrega
                    </Button>
                    <Button
                      type="primary"
                      onClick={() => updateDonationStatus(donation.id, "Received")}
                      loading={updatingDonationId === donation.id}
                      disabled={
                        donation.status === "Received" ||
                        donation.status === "Recebida" ||
                        donation.status === "TemporarilyUnavailable" ||
                        donation.status === "UnavailableAtTheMoment" ||
                        donation.status === "IndisponivelNoMomento"
                      }
                    >
                      Marcar recebida
                    </Button>
                    <Button
                      danger
                      onClick={() => openUnavailableReasonModal(donation.id)}
                      loading={updatingDonationId === donation.id}
                      disabled={
                        donation.status === "Received" ||
                        donation.status === "Recebida" ||
                        donation.status === "TemporarilyUnavailable" ||
                        donation.status === "UnavailableAtTheMoment" ||
                        donation.status === "IndisponivelNoMomento"
                      }
                    >
                      Indisponível no momento
                    </Button>
                  </Space>
                </div>
              </Card>
            ))}
          </div>
        )}
      </Modal>

      <Modal
        title="Motivo do redirecionamento da doação"
        open={unavailableModalOpen}
        onCancel={() => {
          setUnavailableModalOpen(false);
          setPendingUnavailableDonationId(null);
        }}
        onOk={handleConfirmUnavailable}
        okText="Confirmar"
        cancelText="Cancelar"
      >
        <p>Selecione um motivo para o cidadão receber uma justificativa clara.</p>
        <Select
          style={{ width: "100%" }}
          value={selectedUnavailableReason}
          options={unavailableReasonOptions}
          onChange={setSelectedUnavailableReason}
        />
      </Modal>

      <Modal
        title="Gerenciar usuários e cargos"
        open={usersModalOpen}
        onCancel={closeUsersModal}
        footer={[
          <Button key="close-users-modal" onClick={closeUsersModal}>
            Fechar
          </Button>,
        ]}
        width={780}
      >
        {loadingUsers ? (
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
                  </div>

                  <Space>
                    <Select
                      value={user.role || "Citizen"}
                      options={roleOptions}
                      onChange={(role) => onRoleSelectChange(user.userId, role)}
                      style={{ minWidth: 150 }}
                    />
                    <Button
                      type="primary"
                      loading={savingUserRoleId === user.userId}
                      onClick={() => handleSaveUserRole(user)}
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

      <Modal
        title="Gerenciar doadores"
        open={donorsModalOpen}
        onCancel={closeDonorsModal}
        footer={[
          <Button key="close-donors-modal" onClick={closeDonorsModal}>
            Fechar
          </Button>,
        ]}
        width={980}
      >
        {loadingAdminDonors ? (
          <div className="admin-dashboard-loading">
            <Spin size="large" />
          </div>
        ) : adminDonors.length === 0 ? (
          <Empty description="Nenhum doador encontrado." />
        ) : (
          <div className="admin-donor-list">
            {adminDonors.map((donor) => (
              <Card key={donor.id} size="small" className="admin-donor-card">
                <div className="admin-donor-row">
                  <div>
                    <strong>{donor.name}</strong>
                    <p>{donor.emailUser}</p>
                    <p>
                      Doações realizadas: <b>{donor.donationCount}</b>
                    </p>
                  </div>

                  <Space>
                    <Button onClick={() => openEditDonorModal(donor)}>Editar</Button>
                    <Popconfirm
                      title="Excluir doador"
                      description="Tem certeza que deseja excluir este doador?"
                      okText="Excluir"
                      cancelText="Cancelar"
                      onConfirm={() => handleDeleteDonor(donor.userId)}
                    >
                      <Button danger loading={deletingDonorId === donor.userId}>
                        Excluir
                      </Button>
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
        open={editDonorModalOpen}
        onCancel={closeEditDonorModal}
        onOk={handleUpdateDonor}
        confirmLoading={savingDonor}
        okText="Salvar"
        cancelText="Cancelar"
      >
        <Form form={donorForm} layout="vertical">
          <Form.Item name="userId" hidden>
            <Input />
          </Form.Item>

          <Form.Item label="E-mail" name="email" rules={[{ required: true, message: "Informe o e-mail." }]}>
            <Input />
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
            <Select
              options={[
                { value: "Masculino", label: "Masculino" },
                { value: "Feminino", label: "Feminino" },
                { value: "PessoaNaoBinaria", label: "Pessoa não-binária" },
                { value: "PrefiroNaoDizer", label: "Prefiro não dizer" },
              ]}
              allowClear
            />
          </Form.Item>

          <Form.Item label="Raça" name="skinColor">
            <Select
              options={[
                { value: "Branca", label: "Branca" },
                { value: "Preta", label: "Preta" },
                { value: "Amarela", label: "Amarela" },
                { value: "Parda", label: "Parda" },
                { value: "Asiatica", label: "Asiática" },
                { value: "Indigena", label: "Indígena" },
                { value: "PrefiroNaoDizer", label: "Prefiro não dizer" },
              ]}
              allowClear
            />
          </Form.Item>

          <Form.Item label="É PCD" name="isPcd">
            <Select
              options={[
                { value: true, label: "Sim" },
                { value: false, label: "Não" },
              ]}
            />
          </Form.Item>
        </Form>
      </Modal>
    </section>
  );
}
