using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExtranetQz.Areas.Usuario.Models;
using ExtranetQz.Data;

namespace ExtranetQz.Library
{
    public class LUsuario : ListObject
    {
        public LUsuario(
             UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
             _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
            _environment = environment;
            _usersRole = new LExtranetQzRoles();
            _uploadimage = new LUploadimage();
        }
        internal async Task<SignInResult> UserLoginAsync(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Input.Email,
                model.Input.Password, false, lockoutOnFailure: true);
            if (result.Succeeded)
            {

            }
            return result;
        }
        public async Task<List<InputModelRegister>> getTUsuariosAsync(String valor, int id)
        {
            List<TUsers> listUser;
            List<SelectListItem> _listRoles;
            List<InputModelRegister> userList = new List<InputModelRegister>();
            if (valor == null && id.Equals(0))
            {
                listUser = _context.TUsers.ToList();
            }
            else
            {
                if (id.Equals(0))
                {
                    listUser = _context.TUsers.Where(u => u.NID.StartsWith(valor) || u.Name.StartsWith(valor) ||
             u.LastName.StartsWith(valor) || u.Email.StartsWith(valor)).ToList();
                }
                else
                {
                    listUser = _context.TUsers.Where(u => u.ID.Equals(id)).ToList();
                }
            }
            if (!listUser.Count.Equals(0))
            {
                foreach (var item in listUser)
                {
                    _listRoles = await _usersRole.getRole(_userManager, _roleManager, item.IdUser);
                    var user = _context.Users.Where(u => u.Id.Equals(item.IdUser)).ToList().Last();
                    userList.Add(new InputModelRegister
                    {
                        Id = item.ID,
                        ID = item.IdUser,
                        NID = item.NID,
                        Name = item.Name,
                        LastName = item.LastName,
                        Email = item.Email,
                        Role = _listRoles[0].Text,
                        Image = item.Image,
                        IdentityUser = user
                    });
                }
            }
                return userList;
        }
        public async Task DeleteUsuarioAsync(String id)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () => {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var user = _context.Users.Where(u => u.Id.Equals(id)).ToList().Last();
                        var _listRoles = await _usersRole.getRole(_userManager, _roleManager, id);
                        await _userManager.RemoveFromRoleAsync(user, _listRoles[0].Text);
                        var dataUser = _context.TUsers.Where(u => u.IdUser.Equals(id)).ToList().Last();
                        _context.Remove(dataUser);
                        _context.SaveChanges();
                        _context.Remove(user);
                        _context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            });
        }
        public InputModelRegister GetUsuario(String id)
        {
            var userData = _context.TUsers.Where(u => u.IdUser.Equals(id)).ToList();
            var listUser = _userManager.Users.Where(u => u.Id.Equals(id)).ToList().ElementAt(0);
            if (!userData.Count.Equals(0))
            {
                var data = userData.ElementAt(0);
                return new InputModelRegister
                {
                    LastName = data.LastName,
                    Email = listUser.Email,
                    NID = data.NID,
                    Name = data.Name,
                    PhoneNumber = listUser.PhoneNumber,
                    Image = data.Image
                };
            }
            else
            {
                return new InputModelRegister
                {
                    LastName = "",
                    Email = listUser.Email,
                    NID = "",
                    Name = "",
                    PhoneNumber = listUser.PhoneNumber,
                    Image = null
                };
            }
                
        }
        public async Task<String> PerfilFotoAsync(IFormFile AvatarImage, String id)
        {
            String _errorMessage = null;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () => {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        List<TUsers> userData;
                        var imageByte = await _uploadimage.ByteAvatarImageAsync(
                                      AvatarImage, _environment, "images/default.png");
                        using (var dbContext = new ApplicationDbContext())
                        {
                            userData = dbContext.TUsers.Where(u => u.IdUser.Equals(id)).ToList();
                        }
                            
                        if (!userData.Count.Equals(0))
                        {
                            var data = userData.ElementAt(0);
                            var t_user = new TUsers
                            {
                                ID = data.ID,
                                Name = data.Name,
                                LastName = data.LastName,
                                NID = data.NID,
                                Email = data.Email,
                                IdUser = data.IdUser,
                                Image = imageByte,
                            };
                            _context.Update(t_user);
                            _context.SaveChanges();
                        }
                        else
                        {
                            var t_user = new TUsers
                            {

                                Name = null,
                                LastName = null,
                                NID = null,
                                Email = null,
                                IdUser = id,
                                Image = imageByte,
                            };
                            _context.Add(t_user);
                            _context.SaveChanges();
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        _errorMessage = ex.Message;
                        transaction.Rollback();
                    }
                }
            });
            return _errorMessage;
        }
        internal async Task<String> ActualizarAsync(InputModelRegister model, string idGet)
        {
            String _errorMessage = null;
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () => {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var userData = _context.TUsers.Where(u => u.IdUser.Equals(idGet)).ToList();
                    var listUser = _userManager.Users.Where(u => u.Id.Equals(idGet)).ToList().ElementAt(0);
                    try
                    {
                        var email = _userManager.Users.Where(u => u.Email.Equals(model.Email)).ToList().ElementAt(0);
                        if (email == null)
                        {
                            Save();
                        }
                        else
                        {
                            if (email.Id.Equals(idGet))
                            {
                                Save();
                            }
                            else
                            {
                                _errorMessage = "El email ya está registrado";
                            }
                        }
                        void Save()
                        {
                            using (var dbContext = new ApplicationDbContext())
                            {
                                listUser.UserName = model.Email;
                                listUser.Email = model.Email;
                                listUser.PhoneNumber = model.PhoneNumber;
                                dbContext.Update(listUser);
                                dbContext.SaveChanges();

                                if (userData.Count.Equals(0))
                                {
                                    var t_user = new TUsers
                                    {
                                        Name = model.Name,
                                        LastName = model.LastName,
                                        NID = model.NID,
                                        Email = model.Email,
                                        IdUser = listUser.Id,
                                    };
                                    dbContext.Add(t_user);
                                    dbContext.SaveChanges();
                                }
                                else
                                {
                                    var t_user = new TUsers
                                    {
                                        ID = userData[0].ID,
                                        Name = model.Name,
                                        LastName = model.LastName,
                                        NID = model.NID,
                                        Email = model.Email,
                                        IdUser = listUser.Id,
                                        Image = userData[0].Image
                                    };
                                    dbContext.Update(t_user);
                                    dbContext.SaveChanges();
                                }
                            }
                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {

                        _errorMessage = ex.Message;
                        transaction.Rollback();
                    }
                }
            });

            return _errorMessage;
        }
    }
}
