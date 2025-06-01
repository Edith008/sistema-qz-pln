using Microsoft.AspNetCore.Mvc;

namespace ExtranetQz.Areas.Calculadora.Controllers
{
    [Area("Calculadora")]
    public class CalculadoraController : Controller
    {
        public IActionResult Calculadora()
        {
            return View();
        }
    }
}
