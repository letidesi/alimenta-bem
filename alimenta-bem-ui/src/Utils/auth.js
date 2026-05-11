import axios from "axios";

axios.defaults.withCredentials = true;

const SESSION_KEY = "session";

function isSessionStorageAvailable() {
  try {
    const test = "__ss_test__";
    sessionStorage.setItem(test, "1");
    sessionStorage.removeItem(test);
    return true;
  } catch (e) {
    return false;
  }
}

// Fallback em memória para browsers antigos que bloqueiam sessionStorage
// (modo privado de Android antigo, Samsung Internet <6, WebView restrito)
let memorySession = null;
const ssAvailable = isSessionStorageAvailable();

export const saveSession = function(ref) {
  var userId = ref.userId;
  var role = ref.role;
  var expiresAt = ref.expiresAt;
  var data = JSON.stringify({ userId: userId, role: role, expiresAt: expiresAt });
  if (ssAvailable) {
    try { sessionStorage.setItem(SESSION_KEY, data); } catch (e) { /* ignore */ }
  }
  // Sempre salva em memória como garantia
  memorySession = { userId: userId, role: role, expiresAt: expiresAt };
};

export const getSession = function() {
  var session = null;
  if (ssAvailable) {
    try {
      var raw = sessionStorage.getItem(SESSION_KEY);
      if (raw) session = JSON.parse(raw);
    } catch (e) { /* ignorar erro de parse */ }
  }
  if (!session) session = memorySession;

  // Auto-limpar sessão expirada para não deixar dados em memória além do prazo
  if (session && session.expiresAt != null && session.expiresAt <= Math.floor(Date.now() / 1000)) {
    clearSession();
    return null;
  }
  return session;
};

export const clearSession = function() {
  if (ssAvailable) {
    try { sessionStorage.removeItem(SESSION_KEY); } catch (e) { /* ignore */ }
  }
  memorySession = null;
};

export const isSessionExpired = function(session) {
  if (!session || session.expiresAt == null) return true;
  return session.expiresAt <= Math.floor(Date.now() / 1000);
};

export const getUserIdFromSession = function() {
  var s = getSession();
  return s ? s.userId : null;
};

export const getRoleFromSession = function() {
  var s = getSession();
  return s ? s.role : null;
};
