import React, { useState } from 'react';
import axios from 'axios';
import '../Css/Style.css';
import { validateEmailField } from '../Utils/validation';
import PasswordInput from './PasswordInput';

const CreateUser = () => {
    const [userData, setUserData] = useState({
        name: '',
        email: '',
        password: ''
    });

    const [successMessage, setSuccessMessage] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    const [emailError, setEmailError] = useState('');

    const validateEmail = (value) => validateEmailField(value, setEmailError);

    const handleChange = (e) => {
        setUserData({
            ...userData,
            [e.target.name]: e.target.value
        });
    };
    const URL = import.meta.env.VITE_API_BASE_URL;

    const handleSubmit = async (e) => {
        e.preventDefault();
        setSuccessMessage('');
        setErrorMessage('');

        if (!validateEmail(userData.email)) return;

        try {
            await axios.post(`${URL}/user`, userData);
            setSuccessMessage('Usuário criado com sucesso!');
        } catch (error) {
            setErrorMessage('Ocorreu um erro ao criar o usuário.');
        }
    };

    return (
        <div>
            <form className="create-user-form" onSubmit={handleSubmit}>
                <h2>Registrar usuário</h2>
                <div className="form-group">
                    <label>Nome</label>
                    <input type="text" name="name" value={userData.name} onChange={handleChange} />
                </div>
                <div className="form-group">
                    <label>Email</label>
                    <input
                        type="email"
                        name="email"
                        value={userData.email}
                        onChange={handleChange}
                        onBlur={(e) => validateEmail(e.target.value)}
                        required
                    />
                    {emailError && <span className="field-error">{emailError}</span>}
                </div>
                <div className="form-group">
                    <label>Senha</label>
                    <PasswordInput
                        name="password"
                        value={userData.password}
                        onChange={handleChange}
                        required
                    />
                </div>
                <button type="submit" className="submit-btn">Enviar</button>
            </form>

            {successMessage && (
                <div className="success-message">
                    {successMessage}
                </div>
            )}

            {errorMessage && (
                <div className="error-message">
                    {errorMessage}
                </div>
            )}
        </div>
    );
};

export default CreateUser;
