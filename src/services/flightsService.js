// Servicio para comunicarse con la API de vuelos
// Construye las URL de consulta y realiza las peticiones fetch

const API_BASE = import.meta.env.VITE_API_BASE || 'https://localhost:7178/api/v1';

// Construye querystring a partir de un objeto de parámetros
function buildQuery(params) {
  const esc = encodeURIComponent;
  const entries = Object.entries(params)
    .filter(([_, v]) => v !== undefined && v !== null && String(v).trim() !== '')
    .map(([k, v]) => `${esc(k)}=${esc(v)}`);
  return entries.length ? `?${entries.join('&')}` : '';
}

// Normaliza diferentes formas de nombres que pueda devolver la API
function normalizeVuelo(raw) {
  if (!raw || typeof raw !== 'object') return raw;
  const pick = (...keys) => {
    for (const k of keys) {
      if (raw[k] !== undefined && raw[k] !== null) return raw[k];
    }
    return undefined;
  };

  const id = pick('id', 'idVuelo', 'IdVuelo', 'Id', 'ID');
  const origen = pick('origen', 'ciudadOrigen', 'ciudad_origen', 'CiudadOrigen', 'CiudadOrigen');
  const destino = pick('destino', 'ciudadDestino', 'ciudad_destino', 'CiudadDestino');
  const fecha = pick('fecha', 'fechaVuelo', 'fecha_vuelo', 'FechaVuelo');
  const aerolinea = pick('aerolinea', 'aerolinea', 'Aerolinea');
  const numeroVuelo = pick('numeroVuelo', 'numero_vuelo', 'NumeroVuelo');

  return {
    id,
    origen,
    destino,
    fecha,
    aerolinea,
    numeroVuelo,
    // incluir raw por si quieres inspeccionar
    _raw: raw
  };
}

// Obtiene la lista de vuelos según filtros (origen, destino, fecha, aerolinea, numeroVuelo)
export async function obtenerVuelos(filters = {}) {
  const qs = buildQuery(filters);
  const url = `${API_BASE}/vuelos${qs}`;
  const res = await fetch(url);
  if (!res.ok) {
    const text = await res.text();
    throw new Error(`${res.status} ${res.statusText}: ${text}`);
  }
  const data = await res.json();
  // Log para depuración: ver en consola del navegador la respuesta cruda
  console.log('API raw response obtenerVuelos:', data);

  if (Array.isArray(data)) {
    return data.map(normalizeVuelo);
  }
  // si el endpoint devuelve un objeto con propiedad lista o similar
  if (data && Array.isArray(data.result) ) {
    return data.result.map(normalizeVuelo);
  }
  // si devuelve un objeto único
  return normalizeVuelo(data);
}

// Obtiene el detalle de un vuelo por su id
export async function obtenerVueloPorId(id) {
  if (!id) throw new Error('id es requerido');
  const url = `${API_BASE}/vuelos/${encodeURIComponent(id)}`;
  const res = await fetch(url);
  if (!res.ok) {
    const text = await res.text();
    throw new Error(`${res.status} ${res.statusText}: ${text}`);
  }
  const data = await res.json();
  console.log('API raw response obtenerVueloPorId:', data);
  return normalizeVuelo(data);
}
