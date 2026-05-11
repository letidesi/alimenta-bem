import React, { useState } from "react";
import axios from "axios";
import { message } from "antd";
import { extractApiError } from "../Utils/apiError";
import "../Css/Style.css";

const EMPTY_FORM = { name: "", type: "", description: "", cnpj: "" };

const CreateOrganization = () => {
  const [organizationData, setOrganizationData] = useState(EMPTY_FORM);
  const [loading, setLoading] = useState(false);
  const handleChange = (e) => setOrganizationData({ ...organizationData, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    try {
      await axios.post(`${import.meta.env.VITE_API_BASE_URL}/organization`, organizationData);
      message.success("Instituição criada com sucesso!");
      setOrganizationData(EMPTY_FORM);
    } catch (error) {
      message.error(extractApiError(error, "Ocorreu um erro ao criar a instituição."));
    } finally {
      setLoading(false);
    }
  };

  return (
    <form className="create-user-form" onSubmit={handleSubmit}>
      <h2>Registrar a instituição</h2>
      <div className="form-group">
        <label>Nome</label>
        <input type="text" name="name" value={organizationData.name} onChange={handleChange} required maxLength={150} />
      </div>
      <div className="form-group">
        <label>Tipo da instituição</label>
        <select name="type" value={organizationData.type} onChange={handleChange} required>
          <option value="">Selecione</option>
          <option value="ONG">ONG</option>
          <option value="Escola">Escola</option>
          <option value="Igreja">Igreja</option>
          <option value="Outros">Outros</option>
        </select>
      </div>
      <div className="form-group">
        <label>Descrição</label>
        <textarea name="description" value={organizationData.description} onChange={handleChange} maxLength={500} />
      </div>
      <div className="form-group">
        <label>CNPJ (opcional)</label>
        <input type="text" name="cnpj" value={organizationData.cnpj} onChange={handleChange} placeholder="00.000.000/0000-00" maxLength={18} />
      </div>
      <button type="submit" className="submit-btn" disabled={loading}>{loading ? 'Enviando...' : 'Enviar'}</button>
    </form>
  );
};

export default CreateOrganization;
