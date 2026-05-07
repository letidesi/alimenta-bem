import React, { useState } from "react";
import { Link } from "react-router-dom";
import axios from "axios";
import "../Css/Style.css";

const ForgotPassword = () => {
  const [email, setEmail] = useState("");
  const [submitted, setSubmitted] = useState(false);
  const [loading, setLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrorMessage("");
    setLoading(true);
    try {
      await axios.post(`${import.meta.env.VITE_API_BASE_URL}/user/forgot-password`, { email });
      setSubmitted(true);
    } catch {
      setErrorMessage("Ocorreu um erro. Tente novamente.");
    } finally {
      setLoading(false);
    }
  };

  if (submitted) {
    return (
      <div className="create-user-form">
        <h2>Verifique seu e-mail</h2>
        <p>Se este e-mail estiver cadastrado, você receberá um link para redefinir sua senha em breve.</p>
        <Link to="/login" className="submit-btn-register" style={{ marginTop: "1rem" }}>
          Voltar ao login
        </Link>
      </div>
    );
  }

  return (
    <div>
      <form className="create-user-form" onSubmit={handleSubmit}>
        <h2>Esqueceu a senha</h2>
        <p>Informe seu e-mail e enviaremos um link para redefinir sua senha.</p>

        <div className="form-group">
          <label>E-mail</label>
          <input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>

        <button type="submit" className="submit-btn" disabled={loading}>
          {loading ? "Enviando..." : "Enviar link"}
        </button>

        <div className="form-group-register">
          <Link to="/login" className="submit-btn-register">
            Voltar ao login
          </Link>
        </div>
      </form>

      {errorMessage && <div className="error-message">{errorMessage}</div>}
    </div>
  );
};

export default ForgotPassword;
