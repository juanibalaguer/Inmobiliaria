using Inmobiliaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

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
        public ActionResult Index()
        {
            try
            {
                ViewBag.NuevoId = TempData["NuevoId"];
                ViewBag.NuevaEntidad = TempData["NuevaEntidad"];
                ViewBag.MensajeError = TempData["MensajeError"];
                var contratos = repositorioContrato.ObtenerTodos();
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
        public ActionResult Create()
        {
            try
            {
                ViewBag.inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.inmuebles = repositorioInmueble.ObtenerTodos();
                ViewBag.ErrorDeFecha = TempData["ErrorDeFecha"];
                return View();
            } 
            catch (Exception e)
            {
                throw;
            }
            
        }
        //Traer $ del inmueble para plasmar en el contrato
        // POST: ContratoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {
                if (contrato.FechaFin < contrato.FechaInicio)
                {
                    ViewBag.ErrorDeFecha = "Ingrese un par de fechas válidas";
                    // Falta implementar alguna manera para que se conserven los otros datos del formulario
                    ViewBag.inquilinos = repositorioInquilino.ObtenerTodos();
                    ViewBag.inmuebles = repositorioInmueble.ObtenerTodos();
                    return View(contrato);
                }
                var resultado = repositorioContrato.Create(contrato);
                if (resultado != -1)
                {
                    TempData["NuevoId"] = resultado;
                    TempData["NuevaEntidad"] = "contrato";

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["MensajeError"] = "Hubo un error al crear el contrato.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: ContratoController/Edit/5
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
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var resultado = repositorioContrato.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
