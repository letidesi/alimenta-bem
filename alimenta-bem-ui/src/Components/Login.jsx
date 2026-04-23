import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";
import "../Css/Style.css";

const Login = () => {
  const [loginData, setLoginData] = useState({
    email: "",
    password: "",
  });

  const navigate = useNavigate();

  const [successMessage, setSuccessMessage] = useState("");
  const [errorMessage, setErrorMessage] = useState("");

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

      const role = Array.isArray(roles) ? roles[0] : roles;

      if (role === "Admin") {
        navigate("/admin");
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
          />
        </div>
        <div className="form-group">
          <label>Senha</label>
          <input
            type="password"
            name="password"
            value={loginData.password}
            onChange={handleChange}
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
