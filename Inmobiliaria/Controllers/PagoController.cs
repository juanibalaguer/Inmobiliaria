using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace Inmobiliaria.Controllers
{
    public class PagoController : Controller
    {
        RepositorioPago repositorioPago;
        RepositorioContrato repositorioContrato;
        public PagoController(IConfiguration iconfiguration)
        {
            repositorioPago = new RepositorioPago(iconfiguration);
            repositorioContrato = new RepositorioContrato(iconfiguration);
        }
        // GET: PagoController
        [Authorize]
        public ActionResult Index(int id)
        {
            try
            {
                if (id > 0)
                {
                    ViewBag.IdContrato = id;
                }
                ViewBag.NuevoId = TempData["NuevoId"];
                ViewBag.NuevaEntidad = TempData["NuevaEntidad"];
                ViewBag.MensajeError = TempData["MensajeError"];
                var contratos = repositorioPago.ObtenerTodos(id);
                return View(contratos);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        [Authorize]

        // GET: PagoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PagoController/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            try
            {
                List<Contrato> contratos = new List<Contrato>();
                contratos.Add(repositorioContrato.ObtenerPorId(id));
                ViewBag.contratos = contratos;
                if (id > 0)
                {
                    var pagos = repositorioPago.ObtenerTodos(id);
                    ViewBag.UltimoPago = pagos.Count + 1;
                }
                Pago pago = new Pago();
                pago.IdContrato = id;
                pago.FechaDePago = DateTime.Now;
                ViewBag.IdContrato = id;
                return View(pago);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // POST: PagoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Pago pago)
        {
            try
            {
                var resultado = repositorioPago.Create(pago);
                if (resultado != -1)
                {
                    TempData["NuevoId"] = resultado;
                    TempData["NuevaEntidad"] = "pago";
                    return RedirectToAction("Index", new { id = pago.IdContrato });
                }
                else
                {
                    TempData["MensajeError"] = "Hubo un error al crear el pago.";
                    return View(pago);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: PagoController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            try
            {
                ViewBag.contratos = repositorioContrato.ObtenerTodos();
                var pago = repositorioPago.ObtenerPorId(id);
                return View(pago);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        // POST: PagoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Pago pago)
        {
            try
            {
                var resultado = repositorioPago.Edit(id, pago);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: PagoController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                var pago = repositorioPago.ObtenerPorId(id);
                return View(pago);
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // POST: PagoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var resultado = repositorioPago.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
