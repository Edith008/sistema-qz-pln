using Microsoft.AspNetCore.Mvc;

namespace ExtranetQz.Areas.Factura.Controllers
{
    [Area("Factura")]
    public class FacturaController : Controller
    {
        public IActionResult Factura()
        {
            return View();
        }
    }
}
