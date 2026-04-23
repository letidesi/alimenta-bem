import React, { useState, useEffect } from "react";
import axios from "axios";
import { jwtDecode } from "jwt-decode";
import "../Css/Style.css"; // Importando o CSS corretamente

const CreateDonation = () => {
  const [donationData, setDonationData] = useState({
    naturalPersonId: "",
    organizationId: "",
    itemName: "",
    amountDonated: 0,
  });

  const [institutions, setInstitutions] = useState([]);
  const [loggedDonorName, setLoggedDonorName] = useState("Carregando...");
  const [organizationRequirements, setOrganizationRequirements] = useState([]);
  const [requirementsLoading, setRequirementsLoading] = useState(false);
  const [donationsHistory, setDonationsHistory] = useState([]);
  const [historyLoading, setHistoryLoading] = useState(false);

  useEffect(() => {
    fetch(`${import.meta.env.VITE_API_BASE_URL}/organizations`)
      .then((res) => res.json())
      .then((data) => {
        setInstitutions(data.organizations);
      })
      .catch((err) => console.error("Erro ao carregar instituições:", err));
  }, []);

  useEffect(() => {
    const token = localStorage.getItem("accessToken");

    if (!token) {
      setErrorMessage("Usuário não autenticado para doar.");
      setLoggedDonorName("Não identificado");
      return;
    }

    const decodedToken = jwtDecode(token);
    const userId = decodedToken?.sub || decodedToken?.userId || decodedToken?.id;

    if (!userId) {
      setErrorMessage("Não foi possível identificar o usuário logado.");
      setLoggedDonorName("Não identificado");
      return;
    }

    const fetchLoggedDonor = async () => {
      try {
        const response = await axios.get(
          `${import.meta.env.VITE_API_BASE_URL}/natural-person/${userId}`
        );

        const donor = response.data;
        const donorId = donor?.id || donor?.naturalPersonId || "";
        const donorName = donor?.socialName || donor?.name || "Cidadão logado";

        if (!donorId) {
          setErrorMessage("Complete seu perfil antes de realizar doações.");
          setLoggedDonorName(donorName);
          return;
        }

        setDonationData((prev) => ({
          ...prev,
          naturalPersonId: donorId,
        }));
        setLoggedDonorName(donorName);
        await loadDonationHistory(donorId);
      } catch (error) {
        setErrorMessage("Não foi possível carregar o perfil do doador logado.");
        setLoggedDonorName("Não identificado");
      }
    };

    fetchLoggedDonor();
  }, []);

  const [successMessage, setSuccessMessage] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

  const loadDonationHistory = async (naturalPersonId) => {
    if (!naturalPersonId) return;

    setHistoryLoading(true);

    try {
      const response = await axios.get(
        `${import.meta.env.VITE_API_BASE_URL}/donations/natural-person/${naturalPersonId}`
      );

      setDonationsHistory(response.data?.donations || []);
    } catch {
      setDonationsHistory([]);
    } finally {
      setHistoryLoading(false);
    }
  };

  const statusLabel = (status) => {
    if (status === "Submitted" || status === "Sent" || status === "Enviada") return "Enviada";
    if (status === "InReview" || status === "UnderReview" || status === "EmAnalise") return "Em análise";
    if (status === "ReadyForDelivery" || status === "AwaitingDelivery" || status === "AguardandoEntrega") return "Aguardando entrega";
    if (status === "Received" || status === "Recebida") return "Recebida";
    if (status === "TemporarilyUnavailable" || status === "UnavailableAtTheMoment" || status === "IndisponivelNoMomento") return "Indisponível no momento";
    return status;
  };

  const statusClassName = (status) => {
    if (status === "Received" || status === "Recebida") return "donation-status received";
    if (status === "TemporarilyUnavailable" || status === "UnavailableAtTheMoment" || status === "IndisponivelNoMomento") return "donation-status unavailable";
    if (status === "ReadyForDelivery" || status === "AwaitingDelivery" || status === "AguardandoEntrega") return "donation-status waiting";
    if (status === "InReview" || status === "UnderReview" || status === "EmAnalise") return "donation-status analyzing";
    return "donation-status sent";
  };

  const handleChange = (e) => {
    const { name, value } = e.target;

    setDonationData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleOrganizationChange = async (e) => {
    const selectedOrganizationId = e.target.value;

    setDonationData((prev) => ({
      ...prev,
      organizationId: selectedOrganizationId,
    }));

    if (!selectedOrganizationId) {
      setOrganizationRequirements([]);
      return;
    }

    setRequirementsLoading(true);
    try {
      const response = await axios.get(
        `${import.meta.env.VITE_API_BASE_URL}/organization-requirements/${selectedOrganizationId}`
      );

      const list = response.data?.requirements || [];
      setOrganizationRequirements(list);
    } catch (error) {
      setOrganizationRequirements([]);
    } finally {
      setRequirementsLoading(false);
    }
  };

  const applyRequirementSuggestion = (requirement) => {
    setDonationData((prev) => ({
      ...prev,
      itemName: requirement.itemName,
      amountDonated: requirement.quantity,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSuccessMessage("");
    setErrorMessage("");

    if (!donationData.naturalPersonId) {
      setErrorMessage("Complete seu perfil para realizar doações.");
      return;
    }

    try {
      await axios.post(
        `${import.meta.env.VITE_API_BASE_URL}/donation`,
        donationData
      );
      setSuccessMessage("Doação realizada com sucesso!");
      await loadDonationHistory(donationData.naturalPersonId);
    } catch (error) {
      setErrorMessage("Ocorreu um erro ao realizar a doação.");
    }
  };

  return (
    <div>
      <form className="create-user-form" onSubmit={handleSubmit}>
        <h2>Realizar uma doação</h2>
        <div className="form-group">
          <label>Doador (usuário logado)</label>
          <input type="text" value={loggedDonorName} disabled aria-readonly="true" />
        </div>
        <div className="form-group">
          <label>Instituição</label>
          <select
            name="organizationId"
            value={donationData.organizationId}
            onChange={handleOrganizationChange}
            required
          >
            <option value="">Selecione uma instituição</option>
            {institutions.map((org, index) => (
              <option key={index} value={org.id}>
                {org.name}
              </option>
            ))}
          </select>
        </div>

        {!!donationData.organizationId && (
          <div className="form-group requirement-message-box">
            <label>Necessidades desta instituição</label>

            {requirementsLoading ? (
              <p className="requirement-message-text">Carregando sugestões...</p>
            ) : organizationRequirements.length > 0 ? (
              <ul className="requirement-list">
                {organizationRequirements.map((item) => (
                  <li key={item.id} className="requirement-list-item">
                    <div>
                      <strong>{item.itemName}</strong>
                      <span>
                        Quantidade sugerida: {item.quantity} | Prioridade: {item.type}
                      </span>
                    </div>
                    <button
                      type="button"
                      className="submit-btn-register"
                      onClick={() => applyRequirementSuggestion(item)}
                    >
                      Usar sugestão
                    </button>
                  </li>
                ))}
              </ul>
            ) : (
              <p className="requirement-message-text">
                Esta instituição ainda não publicou necessidades específicas.
              </p>
            )}
          </div>
        )}

        <div className="form-group">
          <label>Nome do item</label>
          <input
            type="text"
            name="itemName"
            value={donationData.itemName}
            onChange={handleChange}
            required
          />
        </div>
        <div className="form-group">
          <label>Quantidade doada</label>
          <input
            type="number"
            name="amountDonated"
            value={donationData.amountDonated}
            onChange={handleChange}
            min={1}
            required
          />
        </div>
        <button type="submit" className="submit-btn">
          Enviar
        </button>
      </form>

      {successMessage && (
        <div className="success-message">{successMessage}</div>
      )}

      {errorMessage && <div className="error-message">{errorMessage}</div>}

      <div className="donation-history-box">
        <h3>Minhas doações</h3>

        {historyLoading ? (
          <p className="requirement-message-text">Carregando histórico...</p>
        ) : donationsHistory.length === 0 ? (
          <p className="requirement-message-text">Você ainda não possui doações registradas.</p>
        ) : (
          <ul className="donation-history-list">
            {donationsHistory.map((donation) => (
              <li key={donation.id} className="donation-history-item">
                <div>
                  <strong>{donation.itemName}</strong>
                  <span>
                    Instituição: {donation.organizationName} | Quantidade: {donation.amountDonated}
                  </span>
                </div>
                <div className="donation-history-status-box">
                  <span className={statusClassName(donation.status)}>
                    {statusLabel(donation.status)}
                  </span>
                  {donation.unavailableMessage && (
                    <p className="donation-unavailable-message">{donation.unavailableMessage}</p>
                  )}
                </div>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
};

export default CreateDonation;
