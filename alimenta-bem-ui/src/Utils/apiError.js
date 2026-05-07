export function extractApiError(error, fallback = "Ocorreu um erro inesperado.") {
  const data = error?.response?.data;
  if (!data) return fallback;

  // { errors: [{ reason: "..." }] } or { errors: [{ errorMessage: "..." }] }
  if (Array.isArray(data.errors) && data.errors.length > 0) {
    const first = data.errors[0];
    return first.reason || first.errorMessage || fallback;
  }

  // { errors: { fieldName: ["msg1", ...], ... } } — FastEndpoints validation dict nested under errors
  if (data.errors && typeof data.errors === "object" && !Array.isArray(data.errors)) {
    const firstKey = Object.keys(data.errors).find((k) => Array.isArray(data.errors[k]) && data.errors[k].length > 0);
    if (firstKey) return data.errors[firstKey][0];
  }

  // { message: "..." }
  if (typeof data.message === "string" && data.message) return data.message;

  // { fieldName: ["msg1", ...], ... } — flat validation dict
  const firstKey = Object.keys(data).find((k) => Array.isArray(data[k]) && data[k].length > 0);
  if (firstKey) return data[firstKey][0];

  return fallback;
}
