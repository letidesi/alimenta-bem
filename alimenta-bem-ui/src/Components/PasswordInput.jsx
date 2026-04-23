import React, { useState } from "react";

const PasswordInput = ({
  name,
  value,
  onChange,
  minLength,
  maxLength,
  required,
}) => {
  const [showPassword, setShowPassword] = useState(false);

  return (
    <div className="password-wrapper">
      <input
        type={showPassword ? "text" : "password"}
        name={name}
        value={value}
        onChange={onChange}
        minLength={minLength}
        maxLength={maxLength}
        required={required}
      />
      <button
        type="button"
        className="password-toggle"
        onClick={() => setShowPassword((current) => !current)}
        aria-label={showPassword ? "Ocultar senha" : "Mostrar senha"}
      >
        {showPassword ? "Ocultar" : "Mostrar"}
      </button>
    </div>
  );
};

export default PasswordInput;
