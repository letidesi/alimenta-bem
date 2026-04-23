import React, { useEffect, useState } from "react";
import axios from "axios";
import { Switch } from "antd";
import "../Css/Style.css";
import { jwtDecode } from "jwt-decode";
import { validateEmailField } from "../Utils/validation";

const UpdateNaturalPerson = () => {
  const [personData, setPersonData] = useState({
    email: "",
    name: "",
    socialName: "",
    age: "",
    birthdayDate: "",
    gender: "",
    skinColor: "",
    isPcd: false,
    userId: null,
  });

  const [userId, setUserId] = useState(null);
  const [successMessage, setSuccessMessage] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [emailError, setEmailError] = useState("");

  const validateEmail = (value) => validateEmailField(value, setEmailError);

  useEffect(() => {
    const token = localStorage.getItem("accessToken");
    if (!token) return;

    const decoded = jwtDecode(token);
    const userId = decoded.sub || decoded.userId || decoded.id;
    setUserId(userId);

    const fetchUserData = async () => {
      try {
        const response = await axios.get(
          `${import.meta.env.VITE_API_BASE_URL}/user/${userId}`
        );

        const user = response.data.user;
        const email = response.data.email;
        const name = response.data.name;

        setPersonData((prev) => ({
          ...prev,
          ...user,
          email: email ?? prev.email,
          name: name ?? prev.name,
          userId: userId,
        }));
      } catch (error) {
        console.error("Erro ao buscar os dados do usuário", error);
      }
    };

    fetchUserData();
  }, []);

  useEffect(() => {
    if (!userId) return;

    const fetchNaturalPerson = async () => {
      try {
        const response = await axios.get(
          `${import.meta.env.VITE_API_BASE_URL}/natural-person/${userId}`
        );
        setPersonData((prev) => ({
          ...prev,
          ...response.data,
        }));
      } catch (error) {
        console.error("Erro ao buscar dados naturalPerson", error);
      }
    };

    fetchNaturalPerson();
  }, [userId]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setPersonData({
      ...personData,
      [name]: value,
    });
  };

  const handlePcdToggle = (checked) => {
    setPersonData({
      ...personData,
      isPcd: checked,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSuccessMessage("");
    setErrorMessage("");

    if (!validateEmail(personData.email)) return;

    try {
      const payload = {
        ...personData,
        userId: userId,
      };
      await axios.put(
        `${import.meta.env.VITE_API_BASE_URL}/natural-person`,
        payload
      );
      setSuccessMessage("Perfil completado com sucesso!");
    } catch (error) {
      setErrorMessage("Ocorreu um erro ao completar o perfil.");
    }
  };

  return (
    <div>
      <form className="create-user-form" onSubmit={handleSubmit}>
        <h2>Completar Perfil</h2>
        <div className="form-group">
          <label>E-mail</label>
          <input
            type="email"
            name="email"
            value={personData.email}
            onChange={(e) =>
              setPersonData({ ...personData, email: e.target.value })
            }
            onBlur={(e) => validateEmail(e.target.value)}
            required
          />
          {emailError && <span className="field-error">{emailError}</span>}
        </div>
        <div className="form-group">
          <label>Nome Completo</label>
          <input
            type="text"
            name="name"
            value={personData.name}
            onChange={(e) =>
              setPersonData({ ...personData, name: e.target.value })
            }
          />
        </div>
        <div className="form-group">
          <label>Nome social</label>
          <input
            type="text"
            name="socialName"
            value={personData.socialName}
            onChange={(e) =>
              setPersonData({ ...personData, socialName: e.target.value })
            }
          />
        </div>
        <div className="form-group">
          <label>Idade</label>
          <input
            type="number"
            name="age"
            value={personData.age}
            onChange={handleChange}
            min={0}
          />
        </div>
        <div className="form-group">
          <label>Data de nascimento</label>
          <input
            type="date"
            name="birthdayDate"
            value={personData.birthdayDate}
            onChange={handleChange}
          />
        </div>
        <div className="form-group">
          <label>Gênero</label>
          <select
            name="gender"
            value={personData.gender}
            onChange={handleChange}
          >
            <option value="">Selecione</option>
            <option value="Masculino">Masculino</option>
            <option value="Feminino">Feminino</option>
            <option value="PessoaNaoBinaria">Pessoa não-binária</option>
            <option value="PrefiroNaoDizer">Prefiro não dizer</option>
          </select>
        </div>
        <div className="form-group">
          <label>Raça</label>
          <select
            name="skinColor"
            value={personData.skinColor}
            onChange={handleChange}
          >
            <option value="">Selecione</option>
            <option value="Branca">Branca</option>
            <option value="Preta">Preta</option>
            <option value="Amarela">Amarela</option>
            <option value="Parda">Parda</option>
            <option value="Asiatica">Asiática</option>
            <option value="Indigena">Indígena</option>
            <option value="PessoaNaoBinaria">Pessoa não-binária</option>
            <option value="PrefiroNaoDizer">Prefiro não dizer</option>
          </select>
        </div>
        <div className="form-group pcd-control-group">
          <div className="pcd-control-header">
            <div>
              <label htmlFor="pcd-switch">Pessoa com deficiência (PCD)</label>
              <span
                className={`pcd-status-text ${personData.isPcd ? "on" : "off"}`}
              >
                Status: {personData.isPcd ? "Ativado" : "Desativado"}
              </span>
            </div>
            <Switch
              id="pcd-switch"
              checked={personData.isPcd}
              onChange={handlePcdToggle}
              aria-label="Alternar status PCD"
              className="pcd-switch"
            />
          </div>
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

export default UpdateNaturalPerson;
