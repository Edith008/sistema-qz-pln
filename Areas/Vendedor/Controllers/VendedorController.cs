using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ExtranetQz.Areas.Vendedor.Models;
using ExtranetQz.Data;  // Ajusta a tu namespace real
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExtranetQz.Areas.ClienteFac.Models;


namespace ExtranetQz.Areas.Vendedor.Controllers
{
    [Area("Vendedor")]
    [Route("Vendedor/[controller]/[action]")]
    [Authorize]
    public class VendedorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VendedorController> _logger;

        public VendedorController(ApplicationDbContext context, ILogger<VendedorController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // Método para mostrar productos del vendedor
        public async Task<IActionResult> ProductosVendedor()
        {
            try
            {
                //  1) Usuario autenticado
                var username = User.Identity.Name;

                //  2) Busca su NID (ID de vendedor)
                var user = await _context.TUsers.FirstOrDefaultAsync(u => u.Email == username);
                if (user == null)
                {
                    _logger.LogWarning("Usuario no encontrado.");
                    return View("Error");
                }

                var vendedorId = user.NID;

                // 3) Valida vendedor autorizado
                var vendedoresAutorizados = new[] { "10", "20", "27" };
                if (!vendedoresAutorizados.Contains(vendedorId))
                {
                    _logger.LogWarning($"Vendedor {vendedorId} NO autorizado.");
                    return View("Error");
                }

                // 4) Llama al API
                using var httpClient = new HttpClient();
                var url = $"http://51.161.9.55:3000/sapb1/vendedor-productos/{vendedorId}";

                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error en API: {response.StatusCode}");
                    return View("Error");
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ProductosVendedorResponse>(json);

                ViewBag.ArchivoRuta = apiResponse.archivo?.rutaArchivo;

                // Forzar vista correcta si la carpeta es /Vendedor/
                return View("ProductosVendedor", apiResponse.resultados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ProductosVendedorController");
                return View("Error");
            }
        }

        // Método para mostrar clientes del vendedor
        public async Task<IActionResult> ClientesVendedor()
        {
            try
            {
                var username = User.Identity.Name;
                var user = await _context.TUsers.FirstOrDefaultAsync(u => u.Email == username);
                if (user == null)
                {
                    _logger.LogWarning("Usuario no encontrado.");
                    return View("Error");
                }

                var vendedorId = user.NID;

                var vendedoresAutorizados = new[] { "10", "20", "27" };
                if (!vendedoresAutorizados.Contains(vendedorId))
                {
                    _logger.LogWarning($"Vendedor {vendedorId} NO autorizado.");
                    return View("Error");
                }

                using var httpClient = new HttpClient();
                var url = $"http://51.161.9.55:3000/sapb1/vendedor-clientes/{vendedorId}";

                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error en API: {response.StatusCode}");
                    return View("Error");
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<ClientesVendedorResponse>(json);

                ViewBag.ArchivoRuta = apiResponse.archivo?.rutaArchivo;

                return View("ClientesVendedor", apiResponse.resultados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ClientesVendedor");
                return View("Error");
            }
        }

        //Método para mostrar  facturas del vendedor
        public async Task<IActionResult> FacturasVendedor()
        {
            try
            {
                var username = User.Identity.Name;
                var user = await _context.TUsers.FirstOrDefaultAsync(u => u.Email == username);
                if (user == null)
                {
                    _logger.LogWarning("Usuario no encontrado.");
                    return View("Error");
                }

                var vendedorId = user.NID;

                var vendedoresAutorizados = new[] { "10", "20", "27" };
                if (!vendedoresAutorizados.Contains(vendedorId))
                {
                    _logger.LogWarning($"Vendedor {vendedorId} NO autorizado.");
                    return View("Error");
                }

                using var httpClient = new HttpClient();
                var url = $"http://51.161.9.55:3000/sapb1/vendedor-facturas-preventistas/{vendedorId}";

                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error en API: {response.StatusCode}");
                    return View("Error");
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<FacturasVendedorResponse>(json);

                ViewBag.ArchivoRuta = apiResponse.archivo?.rutaArchivo;

                return View("FacturasVendedor", apiResponse.resultados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en FacturasVendedor");
                return View("Error");
            }
        }


        public async Task<IActionResult> VerFacturaPdf(int docNum)
        {
            using var httpClient = new HttpClient();
            var url = $"http://51.161.9.55:3000/sapb1/cliente-factura/{docNum}";

            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var facturaDetalle = Newtonsoft.Json.JsonConvert.DeserializeObject<VClienteFacturaDetalleResponse>(json);

            var rutaPdf = facturaDetalle.archivo.rutaArchivo;
            if (!rutaPdf.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                rutaPdf = "http://" + rutaPdf;

            return Redirect(rutaPdf);
        }




    }
}

