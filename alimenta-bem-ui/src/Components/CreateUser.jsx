import React, { useState } from 'react';
import axios from 'axios';
import { message } from 'antd';
import '../Css/Style.css';
import { validateEmailField } from '../Utils/validation';
import PasswordInput from './PasswordInput';

const CreateUser = () => {
    const [userData, setUserData] = useState({ name: '', email: '', password: '' });
    const [emailError, setEmailError] = useState('');

    const validateEmail = (value) => validateEmailField(value, setEmailError);

    const handleChange = (e) => setUserData({ ...userData, [e.target.name]: e.target.value });

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!validateEmail(userData.email)) return;
        try {
            await axios.post(`${import.meta.env.VITE_API_BASE_URL}/user`, userData);
            message.success('Usuário criado com sucesso!');
        } catch (error) {
            const msg = error?.response?.data?.errors?.[0]?.reason
                || error?.response?.data?.message
                || 'Ocorreu um erro ao criar o usuário.';
            message.error(msg);
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
            <button type="submit" className="submit-btn">Enviar</button>
        </form>
    );
};

export default CreateUser;
