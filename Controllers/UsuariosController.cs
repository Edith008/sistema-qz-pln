//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;

//namespace ExtranetQz.Controllers
//{
//    //[Route("[controller]")]
//    public class ExtranetQzController : Controller
//    {
//        //[HttpGet]
//        //[Route("/ExtranetQz/Alex")]
//        //[HttpGet("[controller]/[action]/{data:double}")]
//        public IActionResult Index(double data)
//        {
//            //var url = Url.Action("Metodo", "ExtranetQz",new { age=25,name= "Alex"});
//            //return View("Index",data);
//            var url = Url.RouteUrl("Alex", new { age = 25, name = "Alex" });
//            return Redirect(url);
//        }
//        [HttpGet("[controller]/[action]",Name ="Alex")]
//        public IActionResult Metodo(int code)
//        {
//            var data = $"Codigo de estado {code}";
//            return View("Index",data);
//        }
//    }
//}