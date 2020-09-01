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
    public class InquilinoController : Controller
    {
        RepositorioInquilino repositorioInquilino;
        public InquilinoController(IConfiguration iconfiguration)
        {
            repositorioInquilino = new RepositorioInquilino(iconfiguration);
        }
        // GET: InquilinoController
        public ActionResult Index()
        {
            try
            {
                var inquilinos = repositorioInquilino.ObtenerTodos();
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: InquilinoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {
                repositorioInquilino.Create(inquilino);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: InquilinoController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var inquilino = repositorioInquilino.ObtenerPorId(id);
                return View(inquilino);
            } catch (Exception e)
            {
                throw;
            }
            
        }

        // POST: InquilinoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repositorioInquilino.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}
