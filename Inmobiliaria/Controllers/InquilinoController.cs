using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Inmobiliaria.Controllers
{
    public class InquilinoController : Controller
    {
        RepositorioInquilino repositorioInquilino;
        public InquilinoController(IConfiguration iconfiguration)
        {
            repositorioInquilino = new RepositorioInquilino(iconfiguration);
        }
        // GET: InquilinoController
        [Authorize]
        public ActionResult Index(int pagina)
        {
            try
            {
                ViewBag.NuevoId = TempData["NuevoId"];
                ViewBag.NuevaEntidad = TempData["NuevaEntidad"];
                ViewBag.MensajeError = TempData["MensajeError"];
                var inquilinos = repositorioInquilino.ObtenerTodosPorPagina(pagina);
                return View(inquilinos);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        // GET: InquilinoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InquilinoController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: InquilinoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                var resultado = repositorioInquilino.Create(inquilino);
                if (resultado != -1)
                {
                    TempData["NuevoId"] = resultado;
                    TempData["NuevaEntidad"] = "inquilino";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["MensajeError"] = "Hubo un error al crear el inquilino.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: InquilinoController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            try
            {
                var inquilino = repositorioInquilino.ObtenerPorId(id);
                return View(inquilino);
            }
            catch (Exception e)
            {
                throw;
            }

        }

        // POST: InquilinoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inquilino inquilino)
        {
            try
            {
                repositorioInquilino.Edit(id, inquilino);
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
                var inquilino = repositorioInquilino.ObtenerPorId(id);
                return View(inquilino);
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
                var resultado = repositorioInquilino.Delete(id);
                if (resultado == -1)
                {
                    TempData["MensajeError"] = "El inquilino no pudo ser eliminado. Verifique si está asociado a un contrato.";
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
