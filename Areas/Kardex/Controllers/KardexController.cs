//using System.Linq;
//using ExtranetQz.Areas.Kardex.Data;
//using ExtranetQz.Areas.Kardex.Models;
//using Microsoft.AspNetCore.Mvc;

//public class KardexController : Controller
//{
//    private readonly KardexDbContext _context;

//    public KardexController(KardexDbContext context)
//    {
//        _context = context;
//    }

//    public IActionResult Kardex()
//    {
//        var movimientos = _context.Movimientos.ToList();
//        return View(movimientos);
//    }
//}

using Microsoft.AspNetCore.Mvc;

namespace ExtranetQz.Areas.Kardex.Controllers
{
    [Area("Kardex")]
    public class KardexController : Controller
    {
        public IActionResult Kardex()
        {
            return View();
        }
    }
}  

