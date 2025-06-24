
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExtranetQz.Areas.Kardex.Models;  // Ajusta el namespace según tu proyecto
using System.Collections.Generic;

namespace ExtranetQz.Areas.Kardex.Controllers
{
    [Area("Kardex")]
    public class KardexController : Controller
    {
        public async Task<IActionResult> Kardex()
        {
            using var httpClient = new HttpClient();

            var response = await httpClient.GetAsync("http://51.161.9.55:3000/api/kardex");
            if (!response.IsSuccessStatusCode)
            {
                // Manejo de error
                return View("Error");
            }

            var json = await response.Content.ReadAsStringAsync();

            // Deserializamos con Newtonsoft.Json
            var kardexResponse = JsonConvert.DeserializeObject<KardexResponse>(json);

            // Enviar la lista de movimientos a la vista
            return View(kardexResponse.resultados);
        }
    }
}
