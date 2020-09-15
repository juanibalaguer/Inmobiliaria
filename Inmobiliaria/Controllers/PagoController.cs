using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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
        public ActionResult Index()
        {
            try
            {
                ViewBag.NuevoId = TempData["NuevoId"];
                ViewBag.NuevaEntidad = TempData["NuevaEntidad"];
                ViewBag.MensajeError = TempData["MensajeError"];
                var contratos = repositorioPago.ObtenerTodos();
                return View(contratos);
            } catch (Exception e)
            {
                throw;
            }
        }

        // GET: PagoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PagoController/Create
        public ActionResult Create()
        {
            try
            {
                ViewBag.contratos = repositorioContrato.ObtenerTodos();
                return View();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        // POST: PagoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago)
        {
            try
            {
                var resultado = repositorioPago.Create(pago);
                if (resultado != -1)
                {
                    TempData["NuevoId"] = resultado;
                    TempData["NuevaEntidad"] = "pago";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["MensajeError"] = "Hubo un error al crear el pago.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: PagoController/Edit/5
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
        public ActionResult Delete(int id)
        {
            var pago = repositorioPago.ObtenerPorId(id);
            return View(pago);
        }

        // POST: PagoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var resultado = repositorioPago.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
