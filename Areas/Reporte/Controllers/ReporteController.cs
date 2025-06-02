//using Microsoft.AspNetCore.Mvc;

//namespace ExtranetQz.Areas.Reporte.Controllers
//{
//    [Area("Reporte")]
//    public class ReporteController : Controller
//    {
//        public IActionResult Reporte()
//        {
//            return View();
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ExtranetQz.Areas.Reporte.Controllers
{
    [Area("Reporte")]
    public class ReporteController : Controller
    {
        public IActionResult Reporte()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerarReporte(string tipoReporte)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(new { consulta = tipoReporte });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://51.161.9.55:3000/api-openai/consulta", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonConvert.DeserializeObject<JObject>(responseString);

                    var resultados = jsonObject["resultados"]?.ToObject<List<Dictionary<string, object>>>();
                    var totalRegistros = jsonObject["totalRegistros"]?.ToObject<int>() ?? 0;

                    // Extrae archivo 
                    var archivo = jsonObject["archivo"];
                    string rutaArchivo = archivo?["rutaArchivo"]?.ToString();
                    string nombreArchivo = archivo?["nombreArchivo"]?.ToString();

                    ViewBag.Resultados = resultados;
                    ViewBag.TotalRegistros = totalRegistros;
                    ViewBag.RutaArchivo = rutaArchivo;
                    ViewBag.NombreArchivo = nombreArchivo;
                }
                else
                {
                    ViewBag.Error = "Error al generar el reporte.";
                }
            }

            return View("Reporte");
        }
    }
}

