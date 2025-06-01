using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExtranetQz.Areas.Principal.Controllers;
using ExtranetQz.Areas.Usuario.Models;
using ExtranetQz.Data;
using ExtranetQz.Library;

namespace ExtranetQz.Areas.Perfil.Controllers
{
    [Area("Perfil")]
    [Authorize]
    public class PerfilController : Controller
    {
        private static String idGet = null;
        private static IdentityUser user;
        //private static TUsers userData;
        private static InputModelRegister userData;
        private LUploadimage _image;
        private LUsuario _usuarios;
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        private ApplicationDbContext _context;
        private static String _errorMessage = null;

        public PerfilController(UserManager<IdentityUser> userManager,
           SignInManager<IdentityUser> signInManager,
           RoleManager<IdentityRole> roleManager,
           ApplicationDbContext context,
           IWebHostEnvironment environment)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _image = new LUploadimage();
            _usuarios = new LUsuario(userManager, signInManager, roleManager, context, environment);
        }
        public IActionResult Perfil(String id)
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (id != null)
                {
                    if (id.Equals(_userManager.GetUserId(User)))
                    {
                        idGet = id;
                        userData = _usuarios.GetUsuario(id);
                        if (_errorMessage != null)
                        {
                            userData.ErrorMessage = _errorMessage;
                            _errorMessage = null;
                        }
                            return View(userData);
                    }
                    else
                    {
                        return RedirectToAction(nameof(PrincipalController.Principal), "Principal");
                    }
                }
                else
                {
                    return RedirectToAction(nameof(PrincipalController.Principal), "Principal");
                }
                   

            }
            else
            {
                return Redirect("/");
            }
        }
        [HttpPost]
        public IActionResult PerfilFoto(IFormFile AvatarImage)
        {
            if (AvatarImage != null)
            {
                var data = _ = _usuarios.PerfilFotoAsync(AvatarImage, idGet);
                _errorMessage = data.Result;
            }
            else
            {
                _errorMessage = "Seleccione una foto o imagen";
            }
            return Redirect("/Perfil/Perfil?id=" + idGet);
        }
        [HttpPost]
        public IActionResult Actualizar(InputModelRegister model)
        {
            var data = _ = _usuarios.ActualizarAsync(model, idGet);
            _errorMessage = data.Result;
            return Redirect("/Perfil/Perfil?id=" + idGet);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPasswordAsync(InputModelRegister model)
        {
            var user = await _userManager.FindByIdAsync(idGet);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, code, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    _errorMessage = error.Description;
                }
            }
                return Redirect("/Perfil/Perfil?id=" + idGet);
        }
    }
}