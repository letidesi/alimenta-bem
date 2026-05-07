import React, { useState } from 'react';
import axios from 'axios';
import { message } from 'antd';
import { useNavigate } from 'react-router-dom';
import '../Css/Style.css';
import { validateEmailField } from '../Utils/validation';
import { extractApiError } from '../Utils/apiError';
import PasswordInput from './PasswordInput';

const EMPTY_FORM = { name: '', email: '', password: '' };

const CreateUser = () => {
    const [userData, setUserData] = useState(EMPTY_FORM);
    const [emailError, setEmailError] = useState('');
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const validateEmail = (value) => validateEmailField(value, setEmailError);

    const handleChange = (e) => setUserData({ ...userData, [e.target.name]: e.target.value });

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!validateEmail(userData.email)) return;
        setLoading(true);
        try {
            await axios.post(`${import.meta.env.VITE_API_BASE_URL}/user`, userData);
            message.success('Usuário criado com sucesso!');
            setUserData(EMPTY_FORM);
            setTimeout(() => navigate('/login'), 1500);
        } catch (error) {
            message.error(extractApiError(error, 'Ocorreu um erro ao criar o usuário.'));
        } finally {
            setLoading(false);
        }
    };

    return (
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
                <PasswordInput name="password" value={userData.password} onChange={handleChange} required />
            </div>
            <button type="submit" className="submit-btn" disabled={loading}>{loading ? 'Enviando...' : 'Enviar'}</button>
        </form>
    );
};

export default CreateUser;
