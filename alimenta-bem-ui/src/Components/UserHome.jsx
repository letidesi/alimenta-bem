import React, { useEffect, useMemo, useState } from "react";
import axios from "axios";
import { Link } from "react-router-dom";
import "../Css/Style.css";
import { getAuthHeaders, getUserIdFromToken } from "../Utils/auth";
import { getDonationStatusClassName, getDonationStatusLabel } from "../Utils/donationStatus";

export default function UserHome() {
  const [donationsHistory, setDonationsHistory] = useState([]);
  const [historyLoading, setHistoryLoading] = useState(false);
  const [historyError, setHistoryError] = useState("");
  const [statusFilter, setStatusFilter] = useState("all");
  const [institutionFilter, setInstitutionFilter] = useState("all");

  useEffect(() => {
    const loadHistory = async () => {
      const userId = getUserIdFromToken();

      if (!userId) {
        setHistoryError("Faça login novamente para visualizar suas doações.");
        return;
      }

      setHistoryLoading(true);
      setHistoryError("");

      try {
        const donorResponse = await axios.get(
          `${import.meta.env.VITE_API_BASE_URL}/natural-person/${userId}`,
          { headers: getAuthHeaders() }
        );

        const donor = donorResponse.data;
        const donorId = donor?.id || donor?.naturalPersonId;

        if (!donorId) {
          setDonationsHistory([]);
          setHistoryError("Complete seu perfil para acompanhar suas doações.");
          return;
        }

        const donationsResponse = await axios.get(
          `${import.meta.env.VITE_API_BASE_URL}/donations/natural-person/${donorId}`,
          { headers: getAuthHeaders() }
        );

        setDonationsHistory(donationsResponse.data?.donations || []);
      } catch {
        setDonationsHistory([]);
        setHistoryError("Não foi possível carregar o histórico de doações.");
      } finally {
        setHistoryLoading(false);
      }
    };

    loadHistory();
  }, []);

  const statusOptions = useMemo(() => {
    const labels = Array.from(
      new Set(donationsHistory.map((donation) => getDonationStatusLabel(donation.status)))
    );

    return labels.sort((a, b) => a.localeCompare(b, "pt-BR"));
  }, [donationsHistory]);

  const institutionOptions = useMemo(() => {
    const organizations = Array.from(
      new Set(donationsHistory.map((donation) => donation.organizationName).filter(Boolean))
    );

    return organizations.sort((a, b) => a.localeCompare(b, "pt-BR"));
  }, [donationsHistory]);

  const filteredDonations = useMemo(() => {
    return donationsHistory.filter((donation) => {
      const matchesStatus =
        statusFilter === "all" || getDonationStatusLabel(donation.status) === statusFilter;
      const matchesInstitution =
        institutionFilter === "all" || donation.organizationName === institutionFilter;

      return matchesStatus && matchesInstitution;
    });
  }, [donationsHistory, statusFilter, institutionFilter]);

  const clearFilters = () => {
    setStatusFilter("all");
    setInstitutionFilter("all");
  };

  return (
    <div className="user-home-stack">
      <section className="home-hero">
        <h1>Painel do Usuário</h1>
        <p>
          Veja suas informações, complete seu perfil e registre novas doações de
          forma simples em qualquer dispositivo.
        </p>
      </section>

      <section className="donation-history-box">
        <div className="user-home-donation-header">
          <h3>Minhas doações</h3>
          <Link to="/logged-user/create-donation" className="submit-btn-register">
            Realizar nova doação
          </Link>
        </div>

        {historyLoading ? (
          <p className="requirement-message-text">Carregando histórico...</p>
        ) : historyError ? (
          <p className="requirement-message-text">{historyError}</p>
        ) : donationsHistory.length === 0 ? (
          <p className="requirement-message-text">Você ainda não possui doações registradas.</p>
        ) : (
          <>
            <div className="user-home-filters">
              <div className="user-home-filter-field">
                <label>Situação</label>
                <select
                  value={statusFilter}
                  onChange={(event) => setStatusFilter(event.target.value)}
                >
                  <option value="all">Todas as situações</option>
                  {statusOptions.map((status) => (
                    <option key={status} value={status}>
                      {status}
                    </option>
                  ))}
                </select>
              </div>

              <div className="user-home-filter-field">
                <label>Instituição</label>
                <select
                  value={institutionFilter}
                  onChange={(event) => setInstitutionFilter(event.target.value)}
                >
                  <option value="all">Todas as instituições</option>
                  {institutionOptions.map((organization) => (
                    <option key={organization} value={organization}>
                      {organization}
                    </option>
                  ))}
                </select>
              </div>

              <button type="button" className="submit-btn-register" onClick={clearFilters}>
                Limpar filtros
              </button>
            </div>

            <p className="user-home-filter-count">
              Exibindo {filteredDonations.length} de {donationsHistory.length} doação(ões).
            </p>

            {filteredDonations.length === 0 ? (
              <p className="requirement-message-text">
                Nenhuma doação encontrada para os filtros selecionados.
              </p>
            ) : (
              <ul className="donation-history-list">
                {filteredDonations.map((donation) => (
                  <li key={donation.id} className="donation-history-item">
                    <div>
                      <strong>{donation.itemName}</strong>
                      <span>
                        Instituição: {donation.organizationName} | Quantidade: {donation.amountDonated}
                      </span>
                    </div>
                    <div className="donation-history-status-box">
                      <span className={getDonationStatusClassName(donation.status)}>
                        {getDonationStatusLabel(donation.status)}
                      </span>
                      {donation.unavailableMessage && (
                        <p className="donation-unavailable-message">{donation.unavailableMessage}</p>
                      )}
                    </div>
                  </li>
                ))}
              </ul>
            )}
          </>
        )}
      </section>
    </div>
  );
}
