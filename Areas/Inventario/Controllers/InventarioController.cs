using Microsoft.AspNetCore.Mvc;

namespace ExtranetQz.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class InventarioController : Controller
    {
        public IActionResult Inventario()
        {
            return View();
        }
    }
}
