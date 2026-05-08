import React, { useState, useEffect } from "react";
import axios from "axios";
import { message } from "antd";
import { extractApiError } from "../Utils/apiError";
import "../Css/Style.css";

const EMPTY_FORM = { organizationId: "", itemName: "", quantity: 0, type: "" };

const CreateOrganizationRequirement = () => {
  const [requirementData, setRequirementData] = useState(EMPTY_FORM);
  const [institutions, setInstitutions] = useState([]);
  const [loading, setLoading] = useState(false);
  const priorityOptions = ["Alta", "Media", "Baixa"];
  useEffect(() => {
    axios
      .get(`${import.meta.env.VITE_API_BASE_URL}/organizations`)
      .then((res) => setInstitutions(res.data?.organizations || []))
      .catch(() => {});
  }, []);

  const handleChange = (e) => setRequirementData({ ...requirementData, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      await axios.post(
        `${import.meta.env.VITE_API_BASE_URL}/organization-requirement`,
        requirementData,
      );
      message.success("Item necessário criado com sucesso!");
      setRequirementData(EMPTY_FORM);
    } catch (error) {
      message.error(extractApiError(error, "Ocorreu um erro ao criar o item."));
    } finally {
      setLoading(false);
    }
  };

  return (
    <form className="create-user-form" onSubmit={handleSubmit}>
      <h2>Registrar necessidades de doações</h2>
      <div className="form-group">
        <label>Instituição</label>
        <select name="organizationId" value={requirementData.organizationId} onChange={handleChange} required>
          <option value="">Selecione uma instituição</option>
          {institutions.map((org, index) => (
            <option key={index} value={org.id}>{org.name}</option>
          ))}
        </select>
      </div>
      <div className="form-group">
        <label>Nome do item que instituição mais precisa</label>
        <input type="text" name="itemName" value={requirementData.itemName} onChange={handleChange} required />
      </div>
      <div className="form-group">
        <label>Quantidade</label>
        <input type="number" name="quantity" value={requirementData.quantity} onChange={handleChange} min={1} required />
      </div>
      <div className="form-group">
        <label>Prioridade</label>
        <select name="type" value={requirementData.type} onChange={handleChange} required>
          <option value="">Selecione</option>
          {priorityOptions.map((option) => (
            <option key={option} value={option}>{option}</option>
          ))}
        </select>
      </div>
      <button type="submit" className="submit-btn" disabled={loading}>{loading ? 'Enviando...' : 'Enviar'}</button>
    </form>
  );
};

export default CreateOrganizationRequirement;
