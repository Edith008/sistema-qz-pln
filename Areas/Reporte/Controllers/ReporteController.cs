using Microsoft.AspNetCore.Mvc;

namespace ExtranetQz.Areas.Reporte.Controllers
{
    [Area("Reporte")]
    public class ReporteController : Controller
    {
        public IActionResult Reporte()
        {
            return View();
        }
    }
}




