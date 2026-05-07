import React, { useState } from "react";
import { Link, useSearchParams, useNavigate } from "react-router-dom";
import axios from "axios";
import { message } from "antd";
import "../Css/Style.css";
import PasswordInput from "./PasswordInput";
import { extractApiError } from "../Utils/apiError";

const ResetPassword = () => {
  const [searchParams] = useSearchParams();
  const token = searchParams.get("token");
  const navigate = useNavigate();

  const [newPassword, setNewPassword] = useState("");
  const [confirm, setConfirm] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (newPassword !== confirm) {
      message.error("As senhas não coincidem.");
      return;
    }

    if (newPassword.length < 6) {
      message.error("A senha deve ter no mínimo 6 caracteres.");
      return;
    }

    setLoading(true);
    try {
      await axios.post(`${import.meta.env.VITE_API_BASE_URL}/user/reset-password`, {
        token,
        newPassword,
      });
      navigate("/login", { state: { message: "Senha redefinida com sucesso! Faça login." } });
    } catch (error) {
      message.error(extractApiError(error, "Token inválido ou expirado. Solicite um novo link."));
    } finally {
      setLoading(false);
    }
  };

  if (!token) {
    return (
      <div className="create-user-form">
        <h2>Link inválido</h2>
        <p>Este link de recuperação é inválido.</p>
        <Link to="/login" className="submit-btn-register" style={{ marginTop: "1rem" }}>
          Voltar ao login
        </Link>
      </div>
    );
  }

  return (
    <div>
      <form className="create-user-form" onSubmit={handleSubmit}>
        <h2>Redefinir senha</h2>

        <div className="form-group">
          <label>Nova senha</label>
          <PasswordInput
            name="newPassword"
            value={newPassword}
            onChange={(e) => setNewPassword(e.target.value)}
            required
          />
        </div>

        <div className="form-group">
          <label>Confirmar senha</label>
          <PasswordInput
            name="confirm"
            value={confirm}
            onChange={(e) => setConfirm(e.target.value)}
            required
          />
        </div>

        <button type="submit" className="submit-btn" disabled={loading}>
          {loading ? "Salvando..." : "Redefinir senha"}
        </button>
      </form>
    </div>
  );
};

export default ResetPassword;
