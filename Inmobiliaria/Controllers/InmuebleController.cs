using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Inmobiliaria.Controllers
{
    public class InmuebleController : Controller
    {
        RepositorioInmueble repositorioInmueble;
        RepositorioPropietario repositorioPropietario;
        public InmuebleController(IConfiguration iconfiguration)
        {
            repositorioInmueble = new RepositorioInmueble(iconfiguration);
            repositorioPropietario = new RepositorioPropietario(iconfiguration);
        }

        // GET: InmuebleController
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                ViewBag.NuevoId = TempData["NuevoId"];
                ViewBag.NuevaEntidad = TempData["NuevaEntidad"];
                ViewBag.MensajeError = TempData["MensajeError"];
                var inmuebles = repositorioInmueble.ObtenerTodos();
                return View(inmuebles);
            }
            catch (Exception e)
            {

                throw;
            }

        }
        [Route("[controller]/Busqueda/{query}/{estado}/{fechaInicio?}/{fechaFin?}", Name = "Buscar")]
        [Authorize]
        public IActionResult Busqueda(string query, bool estado, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var resultado = repositorioInmueble.Busqueda(query, estado, fechaInicio, fechaFin);
                return Json(new { Datos = resultado });

            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message });

            }
        }

        // GET: InmuebleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InmuebleController/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.propietarios = repositorioPropietario.ObtenerTodos();
            return View();
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                var resultado = repositorioInmueble.Create(inmueble);
                if (resultado != -1)
                {
                    TempData["NuevoId"] = resultado;
                    TempData["NuevaEntidad"] = "inmueble";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["MensajeError"] = "Hubo un error al crear el inmueble.";
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: InmuebleController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            try
            {
                ViewBag.propietarios = repositorioPropietario.ObtenerTodos();
                var inmueble = repositorioInmueble.ObtenerPorId(id);
                return View(inmueble);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        // POST: InmuebleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                repositorioInmueble.Edit(id, inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: InmuebleController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                var inmueble = repositorioInmueble.ObtenerPorId(id);
                return View(inmueble);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        // POST: InmuebleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var resultado = repositorioInmueble.Delete(id);
                if (resultado == -1)
                {
                    TempData["MensajeError"] = "El inmueble no pudo ser eliminado. Verifique si está asociado a un contrato.";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
