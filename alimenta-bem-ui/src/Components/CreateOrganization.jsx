import React, { useState } from "react";
import axios from "axios";
import "../Css/Style.css";

const CreateOrganization = () => {
  const [organizationData, setOrganizationData] = useState({
    name: "",
    type: "",
    description: "",
  });

  const [successMessage, setSuccessMessage] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const token = localStorage.getItem("accessToken");
  
  const handleChange = (e) => {
    const { name, value } = e.target;
    setOrganizationData({
      ...organizationData,
      [name]: value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSuccessMessage("");
    setErrorMessage("");
    try {
      await axios.post(
        `${import.meta.env.VITE_API_BASE_URL}/organization`,
        organizationData,
        {
          headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
          },
        }
      );

      setSuccessMessage("Instituição criada com sucesso!");
    } catch (error) {
      setErrorMessage("Ocorreu um erro ao criar a instituição.");
    }
  };

  return (
    <div>
      <form className="create-user-form" onSubmit={handleSubmit}>
        <h2>Registrar a instituição</h2>
        <div className="form-group">
          <label>Nome</label>
          <input
            type="text"
            name="name"
            value={organizationData.name}
            onChange={handleChange}
            required
          />
        </div>
        <div className="form-group">
          <label>Tipo da instituição</label>
          <input
            type="text"
            name="type"
            value={organizationData.type}
            onChange={handleChange}
            required
          />
        </div>
        <div className="form-group">
          <label>Descrição</label>
          <textarea
            name="description"
            value={organizationData.description}
            onChange={handleChange}
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
    </div>
  );
};

export default CreateOrganization;
