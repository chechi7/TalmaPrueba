using AppiEjercicio.Data;
using Microsoft.AspNetCore.Mvc;
using AppiEjercicio.Models;
using Microsoft.EntityFrameworkCore;

namespace AppiEjercicio.Controllers
{
    public class VuelosController : Controller
    {
        private readonly AppDBContext _appDbContext;
        public VuelosController(AppDBContext appDbContext)
        {
            
            _appDbContext = appDbContext;
        }

       
        [HttpGet]
        public async Task<IActionResult> Lista(string origen = null, string destino = null, DateTime? fecha = null, string aerolinea = null, string numeroVuelo = null)
        {
           
            var query = _appDbContext.Vuelos.AsQueryable();

            // Consulta Ciudad de Origen
            if (!string.IsNullOrWhiteSpace(origen))
                query = query.Where(v => v.CiudadOrigen.Contains(origen));

            // Consulta Ciudad de Destino
            if (!string.IsNullOrWhiteSpace(destino))
                query = query.Where(v => v.CiudadDestino.Contains(destino));

            // Consulta Por fechas
            if (fecha.HasValue)
            {
                var start = fecha.Value.Date;
                var end = start.AddDays(1); 
                query = query.Where(v => v.FechaVuelo >= start && v.FechaVuelo < end);
            }

            // Consulta por Aerolínea
            if (!string.IsNullOrWhiteSpace(aerolinea))
                query = query.Where(v => v.Aerolinea.Contains(aerolinea));

            // Consulta por Número de Vuelo
            if (!string.IsNullOrWhiteSpace(numeroVuelo))
                query = query.Where(v => v.NumeroVuelo.Contains(numeroVuelo));

            // Ordenanar por fecha 
            var lista = await query.OrderBy(v => v.FechaVuelo).ToListAsync();

           
            return View(lista);
        }

        // Acción HTTP GET para ver los detalles de un vuelo por su id
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            
            var vuelo = await _appDbContext.Vuelos.FirstOrDefaultAsync(v => v.IdVuelo == id);
            
            if (vuelo == null) return NotFound();
            
            return View(vuelo);
        }
    }


}
