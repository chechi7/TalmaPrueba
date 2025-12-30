using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AppiEjercicio.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppiEjercicio.Controllers.Api
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VuelosController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _dataPath;

        public VuelosController(IWebHostEnvironment env)
        {
            _env = env;
            _dataPath = Path.Combine(_env.ContentRootPath, "Data", "vuelos.json");
        }

        private async Task<List<Vuelos>?> LoadDataAsync()
        {
            if (!System.IO.File.Exists(_dataPath))
            {
                Console.WriteLine($"Archivo de datos no encontrado: {_dataPath}");
                return null;
            }

            try
            {
                var json = await System.IO.File.ReadAllTextAsync(_dataPath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var result = JsonSerializer.Deserialize<List<Vuelos>>(json, options);
                if (result == null)
                {
                    Console.WriteLine($"La deserialización devolvió null para el archivo {_dataPath}");
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error leyendo o deserializando el archivo {_dataPath}: {ex}");
                return null;
            }
        }

        // GET api/v1/vuelos/raw 
        [HttpGet("raw")]
        public async Task<IActionResult> GetRaw()
        {
            if (!System.IO.File.Exists(_dataPath))
            {
                Console.WriteLine($"Solicitud RAW: archivo de datos no encontrado: {_dataPath}");
                return NotFound(new { message = $"Archivo de datos no encontrado: {_dataPath}" });
            }

            var text = await System.IO.File.ReadAllTextAsync(_dataPath);
            return Content(text, "application/json");
        }

        // GET api/v1/vuelos/count -> devuelve el número de registros cargados
        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            var data = await LoadDataAsync();
            if (data == null) return NotFound(new { message = $"Archivo de datos no encontrado: {_dataPath}" });
            return Ok(new { count = data.Count });
        }

        // GET api/v1/vuelos
        // Filtros
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string origen = null, [FromQuery] string destino = null, [FromQuery] string fecha = null, [FromQuery] string aerolinea = null, [FromQuery] string numeroVuelo = null)
        {
            var data = await LoadDataAsync();
            if (data == null)
            {
                return NotFound(new { message = $"Archivo de datos no encontrado: {_dataPath}" });
            }

            var query = data.AsQueryable();

            if (!string.IsNullOrWhiteSpace(origen))
                query = query.Where(v => v.CiudadOrigen.Contains(origen, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(destino))
                query = query.Where(v => v.CiudadDestino.Contains(destino, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(fecha))
            {
                if (DateTime.TryParse(fecha, out var parsed))
                {
                    var start = parsed.Date;
                    var end = start.AddDays(1);
                    query = query.Where(v => v.FechaVuelo >= start && v.FechaVuelo < end);
                }
            }

            if (!string.IsNullOrWhiteSpace(aerolinea))
                query = query.Where(v => v.Aerolinea.Contains(aerolinea, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(numeroVuelo))
                query = query.Where(v => v.NumeroVuelo.Contains(numeroVuelo, StringComparison.OrdinalIgnoreCase));

            var lista = query.OrderBy(v => v.FechaVuelo)
                              .Select(v => new {
                                  id = v.IdVuelo,
                                  origen = v.CiudadOrigen,
                                  destino = v.CiudadDestino,
                                  fecha = v.FechaVuelo,
                                  aerolinea = v.Aerolinea,
                                  numeroVuelo = v.NumeroVuelo
                              })
                              .ToList();
            return Ok(lista);
        }

        // GET api/v1/vuelos/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await LoadDataAsync();
            if (data == null)
            {
                return NotFound(new { message = $"Archivo de datos no encontrado: {_dataPath}" });
            }

            var vuelo = data.FirstOrDefault(v => v.IdVuelo == id);
            if (vuelo == null) return NotFound();

            var dto = new
            {
                id = vuelo.IdVuelo,
                origen = vuelo.CiudadOrigen,
                destino = vuelo.CiudadDestino,
                fecha = vuelo.FechaVuelo,
                aerolinea = vuelo.Aerolinea,
                numeroVuelo = vuelo.NumeroVuelo
            };

            return Ok(dto);
        }
    }
}
