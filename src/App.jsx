import { useState } from "react";
import "./App.css";
import SearchForm from "./components/SearchForm";
import FlightsTable from "./components/FlightsTable";
import { obtenerVuelos } from "./services/flightsService";

function App() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [flights, setFlights] = useState([]);

  async function handleSearch(filters) {
    setError(null);
    setLoading(true);
    try {
      const data = await obtenerVuelos(filters);
      setFlights(Array.isArray(data) ? data : data ? [data] : []);
    } catch (err) {
      setError(err.message);
      setFlights([]);
    } finally {
      setLoading(false);
    }
  }

  function handleReset() {
    // No traer todos los vuelos: sólo limpiar resultados y estado
    setFlights([]);
    setError(null);
    setLoading(false);
  }

  return (
    <div style={{ padding: 20 }}>
      <h1>Búsqueda de vuelos</h1>
      <SearchForm onSearch={handleSearch} onReset={handleReset} />
      {loading && <p>Cargando...</p>}
      {error && <p style={{ color: "red" }}>{error}</p>}
      <FlightsTable flights={flights} />
    </div>
  );
}

export default App;
