using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ExtranetQz.Areas.Usuario.Models;
using ExtranetQz.Controllers;

namespace ExtranetQz.Areas.Principal.Controllers
{
    [Area("Principal")]
    [Authorize]
    public class PrincipalController : Controller
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;
        public PrincipalController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        //[Authorize(Roles ="Admin")]
       // [Authorize(Policy = "Authorization")]
        public async Task<IActionResult> Principal()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var id = _userManager.GetUserId(User);
                var user = await _userManager.FindByIdAsync(id);
                var roles = await _userManager.GetRolesAsync(user);

                if (!roles.Contains("Admin") &&
                    !roles.Contains("Empleado") &&
                    !roles.Contains("Proveedor") &&
                    !roles.Contains("Cliente") &&
                    !roles.Contains("Vendedor")
                    )
                {
                    return Forbid(); // O RedirectToAction("AccesoDenegado")
                }

                return View();
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");

            }
        }
    }
}