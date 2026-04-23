import React, { useState, useEffect } from "react";
import axios from "axios";
import "../Css/Style.css";

const CreateOrganizationRequirement = () => {
  const [requirementData, setRequirementData] = useState({
    organizationId: "",
    itemName: "",
    quantity: 0,
    type: "",
  });

  const [institutions, setInstitutions] = useState([]);
  const priorityOptions = ["Alta", "Media", "Baixa"];

  useEffect(() => {
    fetch(`${import.meta.env.VITE_API_BASE_URL}/organizations`)
      .then((res) => res.json())
      .then((data) => {
        setInstitutions(data.organizations);
      })
      .catch((err) => console.error("Erro ao carregar instituições:", err));
  }, []);

  function handleOrganizationChange(event) {
    const selectedId = event.target.value;
    setRequirementData((prev) => ({
      ...prev,
      organizationId: selectedId,
    }));
  }

  const [successMessage, setSuccessMessage] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const token = localStorage.getItem("accessToken");

  const handleChange = (e) => {
    const { name, value } = e.target;
    setRequirementData({
      ...requirementData,
      [name]: value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSuccessMessage("");
    setErrorMessage("");
    try {
      await axios.post(
        `${import.meta.env.VITE_API_BASE_URL}/organization-requirement`,
        requirementData,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );
      setSuccessMessage(
        "Item necessário para a instituição foi criado com sucesso!"
      );
    } catch (error) {
      setErrorMessage(
        "Ocorreu um erro ao criar item necessário para a instituição."
      );
    }
  };

  return (
    <div>
      <form className="create-user-form" onSubmit={handleSubmit}>
        <h2>Registrar de necessidades de doações </h2>
        <div className="form-group">
          <label>Instituição</label>
          <select
            name="organizationId"
            value={requirementData.organizationId}
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
        <div className="form-group">
          <label>Nome do item que instituição mais precisa</label>
          <input
            type="text"
            name="itemName"
            value={requirementData.itemName}
            onChange={handleChange}
            required
          />
        </div>
        <div className="form-group">
          <label>Quantidade</label>
          <input
            type="number"
            name="quantity"
            value={requirementData.quantity}
            onChange={handleChange}
            min={1}
            required
          />
        </div>
        <div className="form-group">
          <label>Prioridade</label>
          <select
            name="type"
            value={requirementData.type}
            onChange={handleChange}
            required
          >
            <option value="">Selecione</option>
            {priorityOptions.map((option) => (
              <option key={option} value={option}>
                {option}
              </option>
            ))}
          </select>
        </div>
        <button type="submit" className="submit-btn">
          Enviar
        </button>
      </form>

      {successMessage && (
        <div className="success-message">{successMessage}</div>
      )}

      {errorMessage && <div className="error-message">{errorMessage}</div>}
    </div>
  );
};

export default CreateOrganizationRequirement;
