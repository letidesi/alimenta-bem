const STATUS_GROUPS = {
  submitted:              ["Submitted", "Sent", "Enviada"],
  inReview:               ["InReview", "UnderReview", "EmAnalise"],
  readyForDelivery:       ["ReadyForDelivery", "AwaitingDelivery", "AguardandoEntrega"],
  received:               ["Received", "Recebida"],
  temporarilyUnavailable: ["TemporarilyUnavailable", "UnavailableAtTheMoment", "IndisponivelNoMomento"],
};

const matchStatus = (status, group) => STATUS_GROUPS[group].includes(status);

export const getDonationStatusLabel = (status) => {
  if (matchStatus(status, "submitted"))              return "Enviada";
  if (matchStatus(status, "inReview"))               return "Em análise";
  if (matchStatus(status, "readyForDelivery"))       return "Aguardando entrega";
  if (matchStatus(status, "received"))               return "Recebida";
  if (matchStatus(status, "temporarilyUnavailable")) return "Indisponível no momento";
  return status;
};

// Retorna cor para o componente Tag do Ant Design
export const getDonationStatusColor = (status) => {
  if (matchStatus(status, "received"))               return "green";
  if (matchStatus(status, "readyForDelivery"))       return "blue";
  if (matchStatus(status, "inReview"))               return "gold";
  if (matchStatus(status, "temporarilyUnavailable")) return "volcano";
  return "default";
};

// Retorna classe CSS para componentes sem Ant Design
export const getDonationStatusClassName = (status) => {
  if (matchStatus(status, "received"))               return "donation-status received";
  if (matchStatus(status, "temporarilyUnavailable")) return "donation-status unavailable";
  if (matchStatus(status, "readyForDelivery"))       return "donation-status waiting";
  if (matchStatus(status, "inReview"))               return "donation-status analyzing";
  return "donation-status sent";
};

// Retorna true para status que não permitem mais transições pelo admin
export const isDonationFinalized = (status) =>
  matchStatus(status, "received") || matchStatus(status, "temporarilyUnavailable");
