import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";
import { message } from "antd";
import "../Css/Style.css";
import { validateEmailField } from "../Utils/validation";
import { saveSession } from "../Utils/auth";
import PasswordInput from "./PasswordInput";


const Login = () => {
  const [loginData, setLoginData] = useState({ email: "", password: "" });
  const [emailError, setEmailError] = useState("");
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const validateEmail = (value) => validateEmailField(value, setEmailError);

  const handleChange = (e) => {
    setLoginData({ ...loginData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!validateEmail(loginData.email)) return;
    setLoading(true);
    try {
      const response = await axios.post(`${import.meta.env.VITE_API_BASE_URL}/user/authenticate`, loginData);
      const { userId, role, expiresAt } = response.data;

      saveSession({ userId, role, expiresAt });

      if (role === "Admin") navigate("/admin");
      else if (role === "Developer") navigate("/developer");
      else if (role === "Citizen") navigate("/logged-user");
      else navigate("/login");
    } catch {
      message.error("E-mail ou senha inválidos.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <form className="login-form" onSubmit={handleSubmit}>
        <h2>Login</h2>
        <div className="form-group">
          <label>Email</label>
          <input
            type="email"
            name="email"
            value={loginData.email}
            onChange={handleChange}
            onBlur={(e) => validateEmail(e.target.value)}
          />
          {emailError && <span className="field-error">{emailError}</span>}
        </div>
        <div className="form-group">
          <label>Senha</label>
          <PasswordInput name="password" value={loginData.password} onChange={handleChange} required />
        </div>
        <button type="submit" className="submit-btn" disabled={loading}>{loading ? 'Entrando...' : 'Entrar'}</button>
        <div className="form-group-register">
          <Link to="/create-user" className="submit-btn-register">Registra-se</Link>
          <Link to="/forgot-password" className="submit-btn-register">Esqueceu a senha</Link>
        </div>
      </form>
    </div>
  );
};

export default Login;
