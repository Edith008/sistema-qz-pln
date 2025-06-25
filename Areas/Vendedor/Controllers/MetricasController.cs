
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using ExtranetQz.Data;
using ExtranetQz.Areas.Vendedor.Models;
using System.Net.Http;
using System.Collections.Generic;

namespace ExtranetQz.Areas.Vendedor.Controllers
{
    [Area("Vendedor")]
    [Authorize]
    public class MetricasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MetricasController> _logger;

        public MetricasController(ApplicationDbContext context, ILogger<MetricasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ActionResult> FacturasMetricas()
        {
            try
            {
                var username = User.Identity.Name;
                var user = await _context.TUsers.FirstOrDefaultAsync(u => u.Email == username);

                if (user == null) return View("Error");

                var vendedorId = user.NID;

                var vendedoresAutorizados = new[] { "10", "20", "27" };
                if (!vendedoresAutorizados.Contains(vendedorId))
                    return View("Error");

                // Llama a tu API
                using var httpClient = new HttpClient();
                var url = $"http://51.161.9.55:3000/sapb1/vendedor-facturas-preventistas/{vendedorId}";
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode) return View("Error");

                var json = await response.Content.ReadAsStringAsync();
                var facturasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FacturasVendedorResponse>(json);

                var facturas = facturasResponse.resultados;

                // Agrupa por mes y año para métricas y gráfico
                var metricas = facturas
                    .GroupBy(f => new { f.taxDate.Year, f.taxDate.Month })
                    .Select(g => new
                    {
                        Mes = $"{g.Key.Month}/{g.Key.Year}",
                        Total = g.Sum(f => f.docTotal)
                    })
                    .OrderBy(x => x.Mes)
                    .ToList();

                // Totales y promedio
                ViewBag.FacturasTotales = facturas.Count;

                var montoTotal = facturas.Sum(f => f.docTotal);
                ViewBag.MontoTotal = montoTotal.ToString("N2");

                var cantidadMeses = facturas
                    .Select(f => new { f.taxDate.Year, f.taxDate.Month })
                    .Distinct()
                    .Count();

                var promedioMensual = cantidadMeses > 0 ? montoTotal / cantidadMeses : 0;
                ViewBag.MontoPromedio = promedioMensual.ToString("N2");

                // Para el gráfico
                ViewBag.Labels = metricas.Select(x => x.Mes).ToList();
                ViewBag.Data = metricas.Select(x => x.Total).ToList();

                ViewBag.VendedorId = vendedorId;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en FacturasMetricas");
                return View("Error");
            }
        }




        public async Task<ActionResult> PrediccionesFactura()
        {
            try
            {
                var username = User.Identity.Name;
                var user = await _context.TUsers.FirstOrDefaultAsync(u => u.Email == username);

                if (user == null) return View("Error");

                var vendedorId = user.NID;

                var vendedoresAutorizados = new[] { "10", "20", "27" };
                if (!vendedoresAutorizados.Contains(vendedorId))
                    return View("Error");

                using var httpClient = new HttpClient();
                var url = $"http://51.161.9.55:3000/sapb1/vendedor-facturas-preventistas/{vendedorId}";
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode) return View("Error");

                var json = await response.Content.ReadAsStringAsync();
                var facturasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<FacturasVendedorResponse>(json);

                var facturas = facturasResponse.resultados;

                // Agrupa por mes y año
                var metricas = facturas
                    .GroupBy(f => new { f.taxDate.Year, f.taxDate.Month })
                    .Select(g => new
                    {
                        Mes = $"{g.Key.Month}/{g.Key.Year}",
                        Total = g.Sum(f => f.docTotal)
                    })
                    .OrderBy(x => x.Mes)
                    .ToList();

                // Preparar predicciones usando un promedio simple de incremento porcentual
                var predicciones = new List<decimal>();
                decimal? ultimo = null;
                decimal incrementoAcumulado = 0;
                int cuentaIncrementos = 0;

                foreach (var m in metricas)
                {
                    if (ultimo.HasValue && ultimo.Value > 0)
                    {
                        var incremento = (m.Total - ultimo.Value) / ultimo.Value;
                        incrementoAcumulado += incremento;
                        cuentaIncrementos++;
                    }
                    ultimo = m.Total;
                }

                var incrementoPromedio = cuentaIncrementos > 0 ? incrementoAcumulado / cuentaIncrementos : 0;

                // Predice los siguientes 3 meses usando el último total + incremento promedio acumulado
                var prediccionesLabels = new List<string>();
                var prediccionesData = new List<decimal>();

                var ultimoMes = metricas.LastOrDefault();
                decimal? valorBase = ultimoMes?.Total ?? 0;

                int mes = int.Parse(ultimoMes?.Mes.Split('/')[0] ?? "1");
                int anio = int.Parse(ultimoMes?.Mes.Split('/')[1] ?? DateTime.Now.Year.ToString());

                for (int i = 1; i <= 3; i++)
                {
                    mes++;
                    if (mes > 12)
                    {
                        mes = 1;
                        anio++;
                    }

                    valorBase += valorBase * incrementoPromedio;
                    prediccionesLabels.Add($"{mes}/{anio}");
                    prediccionesData.Add(Math.Round(valorBase ?? 0, 2));
                }

                // Pasar datos a la vista
                ViewBag.Labels = prediccionesLabels;
                ViewBag.Data = prediccionesData;

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PrediccionesFactura");
                return View("Error");
            }
        }







    }
}
