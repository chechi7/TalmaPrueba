using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

public class VuelosController : Controller
{
    private readonly IHttpClientFactory _httpFactory;
    public VuelosController(IHttpClientFactory httpFactory) => _httpFactory = httpFactory;

    public async Task<IActionResult> Index(string origen, string destino, string fecha, string aerolinea, string numeroVuelo)
    {
        var client = _httpFactory.CreateClient("ApiClient");
        var query = new List<string>();
        if (!string.IsNullOrWhiteSpace(origen)) query.Add($"origen={Uri.EscapeDataString(origen)}");
        if (!string.IsNullOrWhiteSpace(destino)) query.Add($"destino={Uri.EscapeDataString(destino)}");
        if (!string.IsNullOrWhiteSpace(fecha)) query.Add($"fecha={Uri.EscapeDataString(fecha)}");
        if (!string.IsNullOrWhiteSpace(aerolinea)) query.Add($"aerolinea={Uri.EscapeDataString(aerolinea)}");
        if (!string.IsNullOrWhiteSpace(numeroVuelo)) query.Add($"numeroVuelo={Uri.EscapeDataString(numeroVuelo)}");
        var qs = query.Count > 0 ? "?" + string.Join("&", query) : string.Empty;

        var vuelos = await client.GetFromJsonAsync<List<Vuelo>>($"/vuelos{qs}");
        return View(vuelos ?? new List<Vuelo>());
    }
}
