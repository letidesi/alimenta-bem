import React, { useEffect, useState } from "react";
import axios from "axios";
import { Switch, message } from "antd";
import "../Css/Style.css";
import { getUserIdFromSession } from "../Utils/auth";
import { validateEmailField } from "../Utils/validation";
import { extractApiError } from "../Utils/apiError";

// Detecta suporte nativo a input[type="date"]
function supportsDateInput() {
  try {
    var input = document.createElement("input");
    input.setAttribute("type", "date");
    input.setAttribute("value", "not-a-date");
    return input.value !== "not-a-date";
  } catch (e) {
    return false;
  }
}

const DATE_INPUT_SUPPORTED = supportsDateInput();

const axiosConfig = { withCredentials: true };

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
  const [emailError, setEmailError] = useState("");
  const [submitting, setSubmitting] = useState(false);
  const [dateError, setDateError] = useState("");

  const validateEmail = (value) => validateEmailField(value, setEmailError);

  useEffect(() => {
    var id = getUserIdFromSession();
    if (!id) return;
    setUserId(id);

    axios
      .get(import.meta.env.VITE_API_BASE_URL + "/user/" + id, axiosConfig)
      .then(function(response) {
        var d = response.data || {};
        // Extrair apenas os campos conhecidos — nunca espalhar response.data inteiro
        setPersonData(function(prev) {
          return Object.assign({}, prev, {
            email: d.email != null ? d.email : prev.email,
            name: d.name != null ? d.name : prev.name,
            userId: id,
          });
        });
      })
      .catch(function() {
        // silently ignore — user sees stale/empty state
      });
  }, []);

  useEffect(() => {
    if (!userId) return;

    axios
      .get(import.meta.env.VITE_API_BASE_URL + "/natural-person/" + userId, axiosConfig)
      .then(function(response) {
        var d = response.data || {};
        // Mapear apenas os campos editáveis — evitar mass assignment de campos da API
        setPersonData(function(prev) {
          return {
            email: prev.email,
            name: d.name != null ? d.name : prev.name,
            socialName: d.socialName != null ? d.socialName : prev.socialName,
            age: d.age != null ? d.age : prev.age,
            birthdayDate: d.birthdayDate != null ? d.birthdayDate : prev.birthdayDate,
            gender: d.gender != null ? d.gender : prev.gender,
            skinColor: d.skinColor != null ? d.skinColor : prev.skinColor,
            isPcd: d.isPcd != null ? d.isPcd : prev.isPcd,
            userId: prev.userId,
          };
        });
      })
      .catch(function() {
        // silently ignore — form stays with default empty state
      });
  }, [userId]);

  const handleChange = (e) => {
    var name = e.target.name;
    var value = e.target.value;
    if (name === "birthdayDate") setDateError("");
    setPersonData(function(prev) {
      return Object.assign({}, prev, { [name]: value });
    });
  };

  const handlePcdToggle = (checked) => {
    setPersonData(function(prev) {
      return Object.assign({}, prev, { isPcd: checked });
    });
  };

  // Valida data no formato AAAA-MM-DD (para fallback de texto)
  const validateDate = (value) => {
    if (!value) return true;
    if (!/^\d{4}-\d{2}-\d{2}$/.test(value)) {
      setDateError("Use o formato AAAA-MM-DD (ex: 1990-05-20)");
      return false;
    }
    setDateError("");
    return true;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validateEmail(personData.email)) return;
    if (!validateDate(personData.birthdayDate)) return;

    setSubmitting(true);
    try {
      // Payload explícito — nunca espalhar personData inteiro para evitar envio de campos extras
      var payload = {
        userId: userId,
        email: personData.email,
        name: personData.name,
        socialName: personData.socialName,
        age: personData.age ? personData.age : null,
        birthdayDate: personData.birthdayDate ? personData.birthdayDate : null,
        gender: personData.gender,
        skinColor: personData.skinColor,
        isPcd: personData.isPcd,
      };
      await axios.put(
        import.meta.env.VITE_API_BASE_URL + "/natural-person",
        payload,
        axiosConfig
      );
      message.success("Perfil completado com sucesso!");
    } catch (error) {
      message.error(extractApiError(error, "Ocorreu um erro ao completar o perfil."));
    } finally {
      setSubmitting(false);
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
              setPersonData(function(prev) { return Object.assign({}, prev, { email: e.target.value }); })
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
            onChange={(e) => setPersonData(function(prev) { return Object.assign({}, prev, { name: e.target.value }); })}
            required
            maxLength={200}
          />
        </div>
        <div className="form-group">
          <label>Nome social</label>
          <input
            type="text"
            name="socialName"
            value={personData.socialName}
            onChange={(e) =>
              setPersonData(function(prev) { return Object.assign({}, prev, { socialName: e.target.value }); })
            }
            maxLength={100}
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
            required
          />
        </div>
        <div className="form-group">
          <label>Data de nascimento</label>
          {DATE_INPUT_SUPPORTED ? (
            <input
              type="date"
              name="birthdayDate"
              value={personData.birthdayDate}
              onChange={handleChange}
              required
            />
          ) : (
            <input
              type="text"
              name="birthdayDate"
              value={personData.birthdayDate}
              onChange={handleChange}
              onBlur={(e) => validateDate(e.target.value)}
              placeholder="AAAA-MM-DD"
              pattern="\d{4}-\d{2}-\d{2}"
              inputMode="numeric"
              required
            />
          )}
          {dateError && <span className="field-error">{dateError}</span>}
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
                className={"pcd-status-text " + (personData.isPcd ? "on" : "off")}
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

        <button type="submit" className="submit-btn" disabled={submitting}>
          {submitting ? "Enviando..." : "Enviar"}
        </button>
      </form>
    </div>
  );
};

export default UpdateNaturalPerson;
