using Inmobiliaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Inmobiliaria.Controllers
{
    public class PropietarioController : Controller
    {
        RepositorioPropietario repositorioPropietario;
        int itemsPorPagina;
        public PropietarioController(IConfiguration iconfiguration)
        {
            repositorioPropietario = new RepositorioPropietario(iconfiguration);
            itemsPorPagina = Convert.ToInt32(iconfiguration["ItemsPorPagina"]);

        }
        // GET: PropietarioController
        public ActionResult Index(int pagina)
        {
            try
            {
                int nroPropietarios = repositorioPropietario.ContarPropietarios();
                if(nroPropietarios % itemsPorPagina != 0)
                {
                    ViewBag.NumeroPaginas = 1 + nroPropietarios / itemsPorPagina;
                } else
                {
                    ViewBag.NumeroPaginas = nroPropietarios / itemsPorPagina;
                }
                ViewBag.NuevoId = TempData["NuevoId"];
                ViewBag.NuevaEntidad = TempData["NuevaEntidad"];
                ViewBag.MensajeError = TempData["MensajeError"];
                var propietarios = repositorioPropietario.ObtenerTodosPorPagina(pagina);
                return View(propietarios);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        // GET: PropietarioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PropietarioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropietarioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                var resultado = repositorioPropietario.Create(propietario);
                if (resultado != -1)
                {
                    TempData["NuevoId"] = resultado;
                    TempData["NuevaEntidad"] = "propietario";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["MensajeError"] = "Hubo un error al crear el propietario.";
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: PropietarioController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                Propietario propietario = repositorioPropietario.ObtenerPorId(id);
                return View(propietario);

            }
            catch (Exception e)
            {
                throw;
            }
        }

        // POST: PropietarioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario)
        {
            try
            {
                repositorioPropietario.Edit(id, propietario);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: PropietarioController/Delete/5
        public ActionResult Delete(int id)
        {
            Propietario propietario = repositorioPropietario.ObtenerPorId(id);
            return View(propietario);

        }

        // POST: PropietarioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propietario propietario)
        {
            try
            {
                var resultado = repositorioPropietario.Delete(id);
                if (resultado == -1)
                {
                    TempData["MensajeError"] = "El propietario no pudo ser eliminado. Verifique si está asociado a un inmueble.";
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
