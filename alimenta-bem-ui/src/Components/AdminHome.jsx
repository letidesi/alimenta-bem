import React, { useCallback, useEffect, useRef, useMemo, useState } from "react";
import axios from "axios";
import {
  Badge,
  Button,
  Card,
  Col,
  Empty,
  Input,
  Pagination,
  Popconfirm,
  Row,
  Select,
  Spin,
  Statistic,
  Tag,
  message,
} from "antd";
import { Link } from "react-router-dom";
import "../Css/Style.css";
import { getAuthHeaders } from "../Utils/auth";
import { isDonationPendingReview } from "../Utils/donationStatus";
import EditRequirementModal from "./Admin/EditRequirementModal";
import UsersModal from "./Admin/UsersModal";
import DonorsModal from "./Admin/DonorsModal";
import OrganizationsModal from "./Admin/OrganizationsModal";
import DonationQueueModal from "./Admin/DonationQueueModal";

const PRIORITY_COLOR = { Alta: "red", Media: "gold", Baixa: "blue" };
const PAGE_SIZE = 20;

export default function AdminHome() {
  const [loading, setLoading] = useState(true);
  const [organizations, setOrganizations] = useState([]);
  const [donors, setDonors] = useState([]);
  const [requirementsByOrganization, setRequirementsByOrganization] = useState([]);
  const [editingRequirement, setEditingRequirement] = useState(null);
  const [usersModalOpen, setUsersModalOpen] = useState(false);
  const [donorsModalOpen, setDonorsModalOpen] = useState(false);
  const [organizationsModalOpen, setOrganizationsModalOpen] = useState(false);
  const [donationsModalOpen, setDonationsModalOpen] = useState(false);
  const [institutionSearch, setInstitutionSearch] = useState("");
  const [itemSearch, setItemSearch] = useState("");
  const [priorityFilter, setPriorityFilter] = useState("all");
  const [currentPage, setCurrentPage] = useState(1);
  const [pendingDonationCount, setPendingDonationCount] = useState(0);

  const hasInitializedPendingRef = useRef(false);
  const previousPendingCountRef = useRef(0);

  const loadPendingDonationsCount = useCallback(async (organizationList, notifyOnIncrease = true) => {
    if (!organizationList?.length) {
      setPendingDonationCount(0);
      previousPendingCountRef.current = 0;
      hasInitializedPendingRef.current = true;
      return;
    }

    const donationLists = await Promise.all(
      organizationList.map(async (organization) => {
        try {
          const response = await axios.get(
            `${import.meta.env.VITE_API_BASE_URL}/donations/organization/${organization.id}`,
            { headers: getAuthHeaders() }
          );

          return response.data?.donations || [];
        } catch {
          return [];
        }
      })
    );

    const nextCount = donationLists.reduce(
      (sum, donations) => sum + donations.filter((donation) => isDonationPendingReview(donation.status)).length,
      0
    );

    setPendingDonationCount(nextCount);

    if (
      notifyOnIncrease &&
      hasInitializedPendingRef.current &&
      nextCount > previousPendingCountRef.current
    ) {
      const difference = nextCount - previousPendingCountRef.current;
      message.info(`Nova doação recebida: ${difference} item(ns) aguardando análise.`);
    }

    previousPendingCountRef.current = nextCount;
    hasInitializedPendingRef.current = true;
  }, []);

  const loadDashboard = async () => {
    setLoading(true);
    try {
      const [organizationResponse, donorResponse] = await Promise.all([
        axios.get(`${import.meta.env.VITE_API_BASE_URL}/organizations`),
        axios.get(`${import.meta.env.VITE_API_BASE_URL}/natural-persons`, { headers: getAuthHeaders() }),
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
            return { ...organization, requirements: response.data?.requirements || [] };
          } catch {
            return { ...organization, requirements: [] };
          }
        })
      );

      setRequirementsByOrganization(requirementsResponses);
      await loadPendingDonationsCount(organizationList, false);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadDashboard();
  }, []);

  useEffect(() => {
    if (!organizations.length) return;

    const timer = setInterval(() => {
      loadPendingDonationsCount(organizations, true);
    }, 20000);

    return () => clearInterval(timer);
  }, [organizations, loadPendingDonationsCount]);

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

  const openEditModal = (organization, requirement) => {
    setEditingRequirement({ ...requirement, organizationId: organization.id });
  };

  const updateRequirementState = (updatedRequirement) => {
    setRequirementsByOrganization((prev) => {
      const cleaned = prev.map((org) => ({
        ...org,
        requirements: org.requirements.filter((req) => req.id !== updatedRequirement.id),
      }));
      return cleaned.map((org) => {
        if (org.id !== updatedRequirement.organizationId) return org;
        return {
          ...org,
          requirements: [
            ...org.requirements,
            {
              id:       updatedRequirement.id,
              itemName: updatedRequirement.itemName,
              quantity: updatedRequirement.quantity,
              type:     updatedRequirement.type,
            },
          ].sort((a, b) => a.itemName.localeCompare(b.itemName)),
        };
      });
    });
  };

  const handleDeleteRequirement = async (requirementId) => {
    try {
      await axios.delete(
        `${import.meta.env.VITE_API_BASE_URL}/organization-requirement/${requirementId}`,
        { headers: getAuthHeaders() }
      );
      setRequirementsByOrganization((prev) =>
        prev.map((org) => ({
          ...org,
          requirements: org.requirements.filter((req) => req.id !== requirementId),
        }))
      );
      message.success("Necessidade removida com sucesso.");
    } catch {
      message.error("Não foi possível remover a necessidade.");
    }
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
                <Button onClick={() => setDonorsModalOpen(true)}>Gerenciar doadores</Button>
                <Button onClick={() => setUsersModalOpen(true)}>Gerenciar usuários</Button>
                <Button onClick={() => setOrganizationsModalOpen(true)}>Gerenciar instituições</Button>
                <Badge count={pendingDonationCount} overflowCount={99}>
                  <Button onClick={() => setDonationsModalOpen(true)}>Gerenciar doações</Button>
                </Badge>
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
                                    <Tag color={PRIORITY_COLOR[requirement.type] || "default"}>
                                      {requirement.type}
                                    </Tag>
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

      <EditRequirementModal
        editingRequirement={editingRequirement}
        organizations={organizations}
        onClose={() => setEditingRequirement(null)}
        onSave={updateRequirementState}
      />
      <UsersModal
        open={usersModalOpen}
        onClose={() => setUsersModalOpen(false)}
      />
      <DonorsModal
        open={donorsModalOpen}
        onClose={() => setDonorsModalOpen(false)}
      />
      <OrganizationsModal
        open={organizationsModalOpen}
        onClose={() => setOrganizationsModalOpen(false)}
        organizations={organizations}
        onRefresh={loadDashboard}
      />
      <DonationQueueModal
        open={donationsModalOpen}
        onClose={() => setDonationsModalOpen(false)}
        organizations={organizations}
        onQueueChanged={() => loadPendingDonationsCount(organizations, false)}
      />
    </section>
  );
}
