using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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
        public ActionResult Index()
        {
            try
            {
                var inmuebles = repositorioInmueble.ObtenerTodos();
                return View(inmuebles);
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

        // GET: InmuebleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InmuebleController/Create
        public ActionResult Create()
        {
            ViewBag.propietarios = repositorioPropietario.ObtenerTodos();
            return View();
        }

        // POST: InmuebleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                repositorioInmueble.Create(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InmuebleController/Edit/5
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
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                repositorioInmueble.Edit(id, inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InmuebleController/Delete/5
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
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repositorioInmueble.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
