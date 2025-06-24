
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExtranetQz.Data; // tu contexto
using ExtranetQz.Areas.ClienteFac.Models; // tu modelo de respuesta

namespace ExtranetQz.Areas.ClienteFac.Controllers
{
    [Area("ClienteFac")]
    [Authorize]
    public class ClienteFacController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClienteFacController> _logger;

        public ClienteFacController(ApplicationDbContext context, ILogger<ClienteFacController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> ClienteFac()
        {
            try
            {
                // ✅ 1) Obtén el nombre de usuario autenticado
                var username = User.Identity.Name;
                _logger.LogInformation($"Usuario autenticado: {username}");

                // ✅ 2) Busca su NID en la base
                var user = await _context.TUsers.FirstOrDefaultAsync(u => u.Email == username);
                if (user == null)
                {
                    _logger.LogWarning("Usuario no encontrado en la base de datos.");
                    return View("Error");
                }

                var nit = user.NID; // <-- Tu columna clave
                _logger.LogInformation($"NIT del usuario: {nit}");

                // ✅ 3) Verifica si el NIT es válido
                var validNits = new[] { "CLM01916", "CLM01915", "CLM01475" };
                if (!validNits.Contains(nit))
                {
                    _logger.LogWarning("NIT no autorizado para ver facturas.");
                    return View("Error");
                }

                // ✅ 4) Consume el API con el NIT válido
                using var httpClient = new HttpClient();
                var url = $"http://51.161.9.55:3000/sapb1/cliente-facturas/{nit}";

                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"API Error: {response.StatusCode}");
                    return View("Error");
                }

                var json = await response.Content.ReadAsStringAsync();
                var facturasResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ClienteFacResponse>(json);

                ViewBag.ArchivoRuta = facturasResponse.archivo?.rutaArchivo;

                return View(facturasResponse.facturas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ClienteFacController");
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> VerFacturaPdf(int docNum)
        {
            using var httpClient = new HttpClient();
            var url = $"http://51.161.9.55:3000/sapb1/cliente-factura/{docNum}";

            var response = await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var facturaDetalle = Newtonsoft.Json.JsonConvert.DeserializeObject<ClienteFacturaDetalleResponse>(json);

            var rutaPdf = facturaDetalle.archivo.rutaArchivo;
            if (!rutaPdf.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                rutaPdf = "http://" + rutaPdf;

            return Redirect(rutaPdf);
        }
    }
}
