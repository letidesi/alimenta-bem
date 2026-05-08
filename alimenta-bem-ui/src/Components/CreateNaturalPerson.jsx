import React, { useState } from 'react';
import axios from 'axios';
import { Switch } from 'antd';
import { message } from 'antd';
import '../Css/Style.css';
import { validateEmailField } from '../Utils/validation';
import PasswordInput from './PasswordInput';
import { extractApiError } from '../Utils/apiError';

const EMPTY_FORM = {
    emailUser: '', password: '', firstName: '', lastName: '',
    socialName: '', age: '', birthdayDate: '', gender: '', skinColor: '', isPcd: false
};

const CreateNaturalPerson = () => {
    const [personData, setPersonData] = useState(EMPTY_FORM);
    const [emailError, setEmailError] = useState('');
    const [loading, setLoading] = useState(false);

    const validateEmail = (value) => validateEmailField(value, setEmailError);

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setPersonData({ ...personData, [name]: type === 'checkbox' ? checked : value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!validateEmail(personData.emailUser)) return;
        setLoading(true);
        try {
            const payload = {
                email: personData.emailUser,
                password: personData.password,
                name: `${personData.firstName} ${personData.lastName}`.trim(),
                socialName: personData.socialName,
                age: personData.age,
                birthdayDate: personData.birthdayDate,
                gender: personData.gender,
                skinColor: personData.skinColor,
                isPcd: personData.isPcd
            };
            await axios.post(`${import.meta.env.VITE_API_BASE_URL}/natural-person/admin`, payload);
            message.success('Doador criado com sucesso!');
            setPersonData(EMPTY_FORM);
        } catch (error) {
            message.error(extractApiError(error, 'Ocorreu um erro ao criar o doador.'));
        } finally {
            setLoading(false);
        }
    };

    return (
        <form className="create-user-form" onSubmit={handleSubmit}>
            <h2>Registrar o doador</h2>
            <div className="form-group">
                <label>E-mail do usuário cadastrado</label>
                <input type="email" name="emailUser" value={personData.emailUser} onChange={handleChange} onBlur={(e) => validateEmail(e.target.value)} required />
                {emailError && <span className="field-error">{emailError}</span>}
            </div>
            <div className="form-group">
                <label>Senha do usuário</label>
                <PasswordInput name="password" value={personData.password} onChange={handleChange} minLength={12} maxLength={128} required />
            </div>
            <div className="form-group">
                <label>Nome</label>
                <input type="text" name="firstName" value={personData.firstName} onChange={handleChange} required />
            </div>
            <div className="form-group">
                <label>Sobrenome</label>
                <input type="text" name="lastName" value={personData.lastName} onChange={handleChange} required />
            </div>
            <div className="form-group">
                <label>Nome social</label>
                <input type="text" name="socialName" value={personData.socialName} onChange={handleChange} />
            </div>
            <div className="form-group">
                <label>Idade</label>
                <input type="number" name="age" value={personData.age} onChange={handleChange} required />
            </div>
            <div className="form-group">
                <label>Data de nascimento</label>
                <input type="date" name="birthdayDate" value={personData.birthdayDate} onChange={handleChange} required />
            </div>
            <div className="form-group">
                <label>Gênero</label>
                <select name="gender" value={personData.gender} onChange={handleChange}>
                    <option value="">Selecione</option>
                    <option value="Masculino">Masculino</option>
                    <option value="Feminino">Feminino</option>
                    <option value="PessoaNaoBinaria">Pessoa não-binária</option>
                    <option value="PrefiroNaoDizer">Prefiro não dizer</option>
                </select>
            </div>
            <div className="form-group">
                <label>Raça</label>
                <select name="skinColor" value={personData.skinColor} onChange={handleChange}>
                    <option value="">Selecione</option>
                    <option value="Branca">Branca</option>
                    <option value="Preta">Preta</option>
                    <option value="Amarela">Amarela</option>
                    <option value="Parda">Parda</option>
                    <option value="Asiatica">Asiática</option>
                    <option value="Indigena">Indígena</option>
                    <option value="PrefiroNaoDizer">Prefiro não dizer</option>
                </select>
            </div>
            <div className="form-group pcd-control-group">
                <div className="pcd-control-header">
                    <div>
                        <label htmlFor="pcd-switch">Pessoa com deficiência (PCD)</label>
                        <span className={`pcd-status-text ${personData.isPcd ? 'on' : 'off'}`}>
                            Status: {personData.isPcd ? 'Ativado' : 'Desativado'}
                        </span>
                    </div>
                    <Switch id="pcd-switch" checked={personData.isPcd} onChange={(checked) => setPersonData({ ...personData, isPcd: checked })} className="pcd-switch" />
                </div>
            </div>
            <button type="submit" className="submit-btn" disabled={loading}>{loading ? 'Enviando...' : 'Enviar'}</button>
        </form>
    );
};

export default CreateNaturalPerson;
