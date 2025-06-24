
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using ExtranetQz.Areas.Calculadora.Models;

namespace ExtranetQz.Areas.Calculadora.Controllers
{
    [Area("Calculadora")]
    public class CalculadoraController : Controller
    {
        public async Task<IActionResult> Calculadora()
        {
            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync("http://51.161.9.55:3000/api/detalle-venta");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Error al obtener datos del API.";
                return View(new List<Resultado>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<DetalleVentaResponse>(json);

            return View(data.resultados ?? new List<Resultado>());
        }
    }
}
