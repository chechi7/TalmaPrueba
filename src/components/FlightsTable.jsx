import React from 'react';

export default function FlightsTable({ flights, onSelect }) {
  if (!flights || flights.length === 0) return <p>No se encontraron vuelos.</p>;

  function formatFecha(fecha) {
    if (!fecha) return '';
    try {
      const d = new Date(fecha);
      if (Number.isNaN(d.getTime())) return String(fecha);
      return d.toLocaleString();
    } catch {
      return String(fecha);
    }
  }

  return (
    <table style={{ width: '100%', borderCollapse: 'collapse' }}>
      <thead>
        <tr>
          <th>Origen</th>
          <th>Destino</th>
          <th>Fecha</th>
          <th>Aerolínea</th>
          <th>Nº Vuelo</th>
        </tr>
      </thead>
      <tbody>
        {flights.map((f) => (
          <tr key={f.id} onClick={() => onSelect && onSelect(f)} style={{ cursor: onSelect ? 'pointer' : 'default' }}>
            <td>{f.origen}</td>
            <td>{f.destino}</td>
            <td>{formatFecha(f.fecha)}</td>
            <td>{f.aerolinea}</td>
            <td>{f.numeroVuelo}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
