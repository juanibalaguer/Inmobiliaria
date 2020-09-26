using Inmobiliaria.Models;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inmobiliaria.Controllers
{
    public class UsuarioController : Controller
    {
        RepositorioUsuario repositorioUsuario;
        IConfiguration iconfiguration;
        IHostingEnvironment enviroment;
        public UsuarioController(IConfiguration iconfiguration, IHostingEnvironment enviroment)
        {
            repositorioUsuario = new RepositorioUsuario(iconfiguration);
            this.iconfiguration = iconfiguration;
            this.enviroment = enviroment;
        }
        // GET: UsuarioController
        [Authorize]
        public ActionResult Index(int pagina)
        {
            try
            {
                ViewBag.NuevoId = TempData["NuevoId"];
                ViewBag.NuevaEntidad = TempData["NuevaEntidad"];
                ViewBag.MensajeError = TempData["MensajeError"];
                var usuarios = repositorioUsuario.ObtenerTodos();
                return View(usuarios);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        // GET: UsuarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsuarioController/Create
        public ActionResult Create()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: UsuarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string hashContraseña = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.Contraseña,
                        salt: System.Text.Encoding.ASCII.GetBytes(iconfiguration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                    usuario.Contraseña = hashContraseña;
                    usuario.Rol = User.IsInRole("Administrador") ? usuario.Rol : (int) roles.Empleado;
                    usuario.AvatarUrl = "/Uploads/default.png";
                    int resultado = repositorioUsuario.Create(usuario);
                    if (usuario.AvatarFile != null && usuario.IdUsuario > 0)
                    {
                        string root = enviroment.WebRootPath;
                        string path = Path.Combine(root, "Uploads");
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);

                        }
                        string fileName = "avatar_" + resultado + Path.GetExtension(usuario.AvatarFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        usuario.AvatarUrl = Path.Combine("/Uploads", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                        {
                            usuario.AvatarFile.CopyTo(stream);
                        }
                        repositorioUsuario.Edit(usuario.IdUsuario, usuario);
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    return View();
                }
            }
            else
            {
                return View();
            }

        }
        [AllowAnonymous]
        public ActionResult Login(string ReturnUrl)

        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginView login)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string returnUrl = String.IsNullOrEmpty(TempData["ReturnUrl"] as string) ? "/Home" : TempData["ReturnUrl"].ToString();
                    var usuario = repositorioUsuario.BuscarPorEmail(login.Usuario);
                    string hashContraseña = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: login.Contraseña,
                            salt: System.Text.Encoding.ASCII.GetBytes(iconfiguration["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8));
                    if (usuario != null && hashContraseña == usuario.Contraseña)
                    {

                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, usuario.Email),
                                new Claim("FullName", usuario.Nombre + " " + usuario.Apellido),
                                new Claim(ClaimTypes.Role, usuario.NombreRol()),
                                new Claim("Avatar", usuario.AvatarUrl),
                            };
                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));
                        TempData.Remove("ReturnUrl");
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ViewBag.ErrorLogin = "Usuario y/o contraseña incorrecto/s.";
                        TempData["ReturnUrl"] = returnUrl;
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }

            catch (Exception e)
            {
                throw;
            }
        }
        [Authorize]
        // GET: Usuarios/Login/
        public ActionResult LogoutModal()
        {
            return PartialView("_LogoutModal");
        }
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(
               CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction(nameof(Index), "Home");
            }
            catch (Exception e)
            {
                throw;
            }
        }
        // GET: UsuarioController/Perfil
        [Authorize]
        public ActionResult Perfil()
        {
            try
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                var usuario = repositorioUsuario.BuscarPorEmail(User.Identity.Name);
                return View(usuario);
            }
            catch (Exception e)
            {
                throw;
            }

        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            try
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                var usuario = repositorioUsuario.ObtenerPorId(id);
                return View(usuario);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        // POST: InquilinoController/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Usuario usuario)
        {
            try
            {
                var resultado = repositorioUsuario.Edit(id, usuario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: InquilinoController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                var usuario = repositorioUsuario.ObtenerPorId(id);
                return View(usuario);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        // POST: InquilinoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var resultado = repositorioUsuario.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
