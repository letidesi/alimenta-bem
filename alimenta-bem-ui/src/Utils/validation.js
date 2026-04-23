const EMAIL_REGEX = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

export const validateEmailField = (value, setError) => {
  const currentValue = value ?? "";
  const valid = EMAIL_REGEX.test(currentValue);

  setError(valid || currentValue === "" ? "" : "Informe um e-mail válido.");
  return valid;
};
