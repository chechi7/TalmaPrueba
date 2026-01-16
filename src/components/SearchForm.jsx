import { useState } from 'react';

export default function SearchForm({ onSearch, onReset }) {
  const [form, setForm] = useState({ origen: '', destino: '', fecha: '', aerolinea: '', numeroVuelo: '' });

  function update(e) {
    const { name, value } = e.target;
    setForm((s) => ({ ...s, [name]: value }));
  }

  function submit(e) {
    e.preventDefault();
    const payload = { ...form };
    if (payload.fecha) {
      payload.fecha = new Date(payload.fecha).toISOString().slice(0, 10);
    }
    onSearch(payload);
  }

  function reset(e) {
    e.preventDefault();
    setForm({ origen: '', destino: '', fecha: '', aerolinea: '', numeroVuelo: '' });
    if (onReset) onReset();
  }

  return (
    <form onSubmit={submit} style={{ display: 'grid', gap: 8, gridTemplateColumns: 'repeat(2, 1fr)' }}>
      <input name="origen" placeholder="Origen" value={form.origen} onChange={update} />
      <input name="destino" placeholder="Destino" value={form.destino} onChange={update} />
      <input name="fecha" type="date" placeholder="Fecha" value={form.fecha} onChange={update} />
      <input name="aerolinea" placeholder="Aerolínea" value={form.aerolinea} onChange={update} />
      <input name="numeroVuelo" placeholder="Número vuelo" value={form.numeroVuelo} onChange={update} />
      <div style={{ display: 'flex', gap: 8 }}>
        <button type="submit">Buscar</button>
        <button onClick={reset}>Limpiar</button>
      </div>
    </form>
  );
}
