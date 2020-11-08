using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

//Paginacion y buscador en lista

namespace Inmobiliaria.Controllers
{
    public class ContratoController : Controller
    {
        RepositorioContrato repositorioContrato;
        RepositorioInmueble repositorioInmueble;
        RepositorioInquilino repositorioInquilino;
        public ContratoController(IConfiguration iconfiguration)
        {
            repositorioContrato = new RepositorioContrato(iconfiguration);
            repositorioInmueble = new RepositorioInmueble(iconfiguration);
            repositorioInquilino = new RepositorioInquilino(iconfiguration);
        }
        // GET: ContratoController
        [Authorize]
        public ActionResult Index(int id)
        {
            try
            {
                if (id > 0)
                {
                    ViewBag.IdInmueble = id;
                }
                ViewBag.NuevoId = TempData["NuevoId"];
                ViewBag.NuevaEntidad = TempData["NuevaEntidad"];
                ViewBag.MensajeError = TempData["MensajeError"];
                var contratos = repositorioContrato.ObtenerTodos(id);
                return View(contratos);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        // GET: ContratoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContratoController/Create
        [Authorize]
        [Route("[Controller]/Create/{query}/{estado}/{fechaInicio?}/{fechaFin?}", Name = "NuevoPorFechas")]
        public ActionResult Create(string query, bool estado, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                ViewBag.inquilinos = repositorioInquilino.ObtenerTodos();
                var inmueblesDisponibles = repositorioInmueble.Busqueda(query, estado, fechaInicio, fechaFin);
                if (inmueblesDisponibles.Count <= 0)
                {
                    TempData["MensajeError"] = "No existen inmuebles para la búsqueda realizada";
                    return RedirectToAction("Index", "Inmueble");
                } 
                ViewBag.inmuebles = inmueblesDisponibles;
                var contrato = new Contrato();
                contrato.FechaInicio = fechaInicio;
                contrato.FechaFin = fechaFin;
                return View(contrato);
            }
            catch (Exception e)
            {
                throw;
            }

        }
        [Authorize]
        public ActionResult Create(int id)
        {
            try
            {
                ViewBag.inquilinos = repositorioInquilino.ObtenerTodos();
                if (id > 0)
                {
                    var inmuebles = new List<Inmueble>();
                    inmuebles.Add(repositorioInmueble.ObtenerPorId(id));
                    ViewBag.inmuebles = inmuebles;
                    ViewBag.IdInmueble = id;
                    return View();
                }
                ViewBag.inmuebles = repositorioInmueble.ObtenerTodos();
                ViewBag.ErrorDeFecha = TempData["ErrorDeFecha"];
                return View();

            }
            catch (Exception e)
            {
                throw;
            }

        }

        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                if (contrato.FechaFin < contrato.FechaInicio)
                {
                    ViewBag.MensajeError = "Ingrese un par de fechas válidas";
                    ViewBag.inquilinos = repositorioInquilino.ObtenerTodos();
                    ViewBag.inmuebles = repositorioInmueble.ObtenerTodos();
                    return View(contrato);
                }
                var inmueble = repositorioInmueble.ObtenerPorId(contrato.IdInmueble);
                if (!inmueble.Estado)
                {
                    ViewBag.MensajeError = "El inmueble seleccionado no se encuentra disponible en este momento";
                    ViewBag.inquilinos = repositorioInquilino.ObtenerTodos();
                    ViewBag.inmuebles = repositorioInmueble.ObtenerTodos();
                    return View(contrato);
                }
                var inmueblesDisponibles = repositorioInmueble.Busqueda("", true, contrato.FechaInicio, contrato.FechaFin);
                List<int> idsInmuebles = new List<int>();
                foreach (var item in inmueblesDisponibles)
                {
                    idsInmuebles.Add(item.Id);
                }
                var contiene = idsInmuebles.Contains(contrato.IdInmueble);
                if (!idsInmuebles.Contains(contrato.IdInmueble))
                {
                    ViewBag.MensajeError = "El inmueble seleccionado está ocupado durante la fecha seleccinada";
                    ViewBag.inquilinos = repositorioInquilino.ObtenerTodos();
                    ViewBag.inmuebles = repositorioInmueble.ObtenerTodos();
                    return View(contrato);
                }
                var resultado = repositorioContrato.Create(contrato);
                if (resultado != -1)
                {
                    TempData["NuevoId"] = resultado;
                    TempData["NuevaEntidad"] = "contrato";

                    return RedirectToAction(nameof(Index), contrato.IdInmueble);
                }
                else
                {
                    TempData["MensajeError"] = "Hubo un error al crear el contrato.";
                    return RedirectToAction(nameof(Index), new { id = contrato.IdInmueble});
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: ContratoController/Edit/5


        [Route("[controller]/BuscarVigentes/{fechaDesde}/{fechaHasta}", Name = "BuscarVigente")]
        [Authorize]
        public IActionResult BuscarVigentes(DateTime fechaDesde, DateTime fechaHasta)
        {
            try
            {
                var resultado = repositorioContrato.ObtenerContratosVigentes(fechaDesde, fechaHasta);
                return Json(new { Datos = resultado });

            }
            catch (Exception e)
            {
                return Json(new { Error = e.Message });

            }
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            try
            {
                ViewBag.inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.inmuebles = repositorioInmueble.ObtenerTodos();
                var contrato = repositorioContrato.ObtenerPorId(id);
                return View(contrato);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // POST: ContratoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Contrato contrato)
        {
            try
            {
                var resultado = repositorioContrato.Edit(id, contrato);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: ContratoController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                var contrato = repositorioContrato.ObtenerPorId(id);
                return View(contrato);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        // POST: ContratoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var resultado = repositorioContrato.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
