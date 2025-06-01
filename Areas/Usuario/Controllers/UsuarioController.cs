using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExtranetQz.Areas.Usuario.Models;
using ExtranetQz.Controllers;
using ExtranetQz.Data;
using ExtranetQz.Library;
using ExtranetQz.Models;

namespace ExtranetQz.Areas.Usuario.Controllers
{
    [Area("Usuario")]
    //[Authorize(Roles = "Admin")]
    public class UsuarioController : Controller
    {
        private SignInManager<IdentityUser> _signInManager;
        private static DataPaginador<InputModelRegister> models;
        private LUsuario usuarios;
        public UsuarioController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _signInManager = signInManager;
            usuarios = new LUsuario(userManager, signInManager, roleManager, context,null);
        }

        [Authorize(Roles = "Admin,Empleado")]
        public IActionResult Usuario(int id, String filtrar, int registros)
        {
            if (_signInManager.IsSignedIn(User))
            {
                Object[] objects = new Object[3];
                var data = usuarios.getTUsuariosAsync(filtrar,0);
                if (0 < data.Result.Count)
                {
                    var url = Request.Scheme + "://" + Request.Host.Value;
                    objects = new LPaginador<InputModelRegister>().paginador(data.Result,
                        id, registros, "Usuario", "Usuario", "Usuario", url);
                }
                else
                {
                    objects[0] = "No hay datos que mostrar";
                    objects[1] = "No hay datos que mostrar";
                    objects[2] = new List<InputModelRegister>();
                }
                models = new DataPaginador<InputModelRegister>
                {
                    List = (List<InputModelRegister>)objects[2],
                    Pagi_info = (String)objects[0],
                    Pagi_navegacion = (String)objects[1],
                    Input = new InputModelRegister(),
                };
                return View(models);
            }
            else
            {
                return Redirect("/");
            }
                
        }
        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUsuario(String id)
        {
            await usuarios.DeleteUsuarioAsync(id);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        public IActionResult UsuarioDefault()
        {
            return RedirectToAction("Usuario", new { id = 1, filtrar = "", registros = 10 });
        }

    }

}