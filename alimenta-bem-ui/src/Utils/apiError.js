export function extractApiError(error, fallback = "Ocorreu um erro inesperado.") {
  const data = error?.response?.data;
  if (!data) return fallback;

  // { errors: [{ reason: "..." }] } or { errors: [{ errorMessage: "..." }] }
  if (Array.isArray(data.errors) && data.errors.length > 0) {
    const first = data.errors[0];
    return first.reason || first.errorMessage || fallback;
  }

  // { message: "..." }
  if (typeof data.message === "string" && data.message) return data.message;

  // { fieldName: ["msg1", ...], ... } — FastEndpoints validation dict
  const firstKey = Object.keys(data).find((k) => Array.isArray(data[k]) && data[k].length > 0);
  if (firstKey) return data[firstKey][0];

  return fallback;
}
