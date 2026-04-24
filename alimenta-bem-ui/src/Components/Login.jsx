import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";
import "../Css/Style.css";
import { validateEmailField } from "../Utils/validation";
import PasswordInput from "./PasswordInput";

function getHighestPriorityRole(roles) {
  const rolePriority = ["Admin", "Developer", "Citizen"];
  return rolePriority.find((role) => roles.includes(role)) || null;
}

const Login = () => {
  const [loginData, setLoginData] = useState({
    email: "",
    password: "",
  });

  const navigate = useNavigate();

  const [successMessage, setSuccessMessage] = useState("");
  const [errorMessage, setErrorMessage] = useState("");
  const [emailError, setEmailError] = useState("");

  const validateEmail = (value) => validateEmailField(value, setEmailError);

  const handleChange = (e) => {
    setLoginData({
      ...loginData,
      [e.target.name]: e.target.value,
    });
  };
  const URL = import.meta.env.VITE_API_BASE_URL;

  const handleSubmit = async (e) => {
    e.preventDefault();
    setSuccessMessage("");
    setErrorMessage("");

    if (!validateEmail(loginData.email)) return;

    try {
      const response = await axios.post(`${URL}/user/authenticate`, loginData);

      const { accesstoken, refreshtoken } = response.data;

      localStorage.setItem("accessToken", accesstoken);
      localStorage.setItem("refreshToken", refreshtoken);

      setSuccessMessage("Login realizado com sucesso!");

      function parseJwt(token) {
        try {
          return JSON.parse(atob(token.split(".")[1]));
        } catch (e) {
          return null;
        }
      }

      const decoded = parseJwt(accesstoken);
      const roles = decoded?.role || decoded?.roles;

      const normalizedRoles = Array.isArray(roles)
        ? roles.map((role) => String(role))
        : roles
          ? [String(roles)]
          : [];

      const role = getHighestPriorityRole(normalizedRoles);

      if (role === "Admin") {
        navigate("/admin");
      } else if (role === "Developer") {
        navigate("/developer");
      } else if (role === "Citizen") {
        navigate("/logged-user");
      } else {
        navigate("/login");
      }
    } catch (error) {
      setErrorMessage("Ocorreu um erro ao realizar o login.");
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
          <PasswordInput
            name="password"
            value={loginData.password}
            onChange={handleChange}
            required
          />
        </div>
        <button type="submit" className="submit-btn">
          Entrar
        </button>

        <div className="form-group-register">
          <Link to="/create-user" className="submit-btn-register">
            Registra-se
          </Link>
        </div>
      </form>

      {successMessage && (
        <div className="success-message">{successMessage}</div>
      )}

      {errorMessage && <div className="error-message">{errorMessage}</div>}
    </div>
  );
};

export default Login;
