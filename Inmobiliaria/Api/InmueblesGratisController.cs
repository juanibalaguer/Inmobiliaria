﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inmobiliaria.Api
{
    [Route("api/[controller]")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InmueblesGratisController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IHostingEnvironment enviroment;

        public InmueblesGratisController(DataContext context, IHostingEnvironment enviroment)
        {
            _context = context;
            this.enviroment = enviroment;
        }

        // GET: api/InmueblesGratis/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inmueble>>> GetInmuebles()
        {
            try
            {
                var inmuebles = await _context.Inmuebles.ToListAsync();
                if (inmuebles.Count > 0)
                {
                    return Ok(inmuebles);
                }
                else return NotFound("No se encontraron inmuebles");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        // GET: api/InmueblesGratis/tipos
        [HttpGet("Tipos")]
        public async Task<ActionResult<IEnumerable<Inmueble>>> GetTipos()
        {
            try
            {
                var inmuebles = await _context.Inmuebles
                    .Select(inmueble => 
                    new { inmueble.Tipo, 
                          count = _context.Inmuebles.Where(i => i.Tipo == inmueble.Tipo)
                    .Count() }).Distinct().ToListAsync();

                if (inmuebles.Count > 0)
                {
                    return Ok(inmuebles);
                }
                else return NotFound("No se encontraron inmuebles");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        // GET: api/InmueblesGratis/ambientes
        [HttpGet("ambientes")]
        public async Task<ActionResult<IEnumerable<Inmueble>>> PrecioPorAmbiente()
        {
            try
            {
                var inmuebles = await _context.Inmuebles
                    .Select(inmueble =>
                    new { inmueble.Ambientes,
                        montoPromedio = _context.Inmuebles.Where(i => i.Ambientes == inmueble.Ambientes).Select(i => i.Precio)
                    .Average()
                    }).Distinct()
                    .OrderBy(inmueble => inmueble.Ambientes)
                    .ToListAsync();

                if (inmuebles.Count > 0)
                {
                    return Ok(inmuebles);
                }
                else return NotFound("No se encontraron inmuebles");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        // GET: api/InmueblesGratis/comercialespormes
        [HttpGet("comercialespormes")]
        public async Task<ActionResult<IEnumerable<Inmueble>>> ComercialesPorMes()
        {
            try
            {

                var contratosPorMes = await _context.Contratos
                    .Include(contrato => contrato.Inmueble)
                    .Select(contrato => new
                    {
                        contrato.FechaInicio.Month,
                        comerciales = _context.Contratos
                        .Where(c => c.FechaInicio.Month == contrato.FechaInicio.Month && c.Inmueble.Uso == "Comercial")// && contrato.Inmueble.Uso == "Comercial")
                        .Count()
                    }).Distinct()
                    .OrderBy(contrato => contrato.Month)
                    .ToListAsync();
             

                if (contratosPorMes.Count > 0)
                {
                    return Ok(contratosPorMes);
                }
                else return NotFound("No se encontraron inmuebles");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }
        // GET: api/InmueblesGratis/residencialespormes
        [HttpGet("residencialespormes")]
        public async Task<ActionResult<IEnumerable<Inmueble>>> ResidencialesPorMes()
        {
            try
            {

                var contratosPorMes = await _context.Contratos
                    .Include(contrato => contrato.Inmueble)
                    .Select(contrato => new
                    {
                        contrato.FechaInicio.Month,
                        residenciales = _context.Contratos
                        .Where(c => c.FechaInicio.Month == contrato.FechaInicio.Month && c.Inmueble.Uso == "Residencial")
                        .Count()
                    }).Distinct()
                    .OrderBy(contrato => contrato.Month)
                    .ToListAsync();


                if (contratosPorMes.Count > 0)
                {
                    return Ok(contratosPorMes);
                }
                else return NotFound("No se encontraron inmuebles");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        // GET: api/Inmuebles/
        /*   [HttpGet("Vigentes/")]
           public async Task<ActionResult<IEnumerable<Inmueble>>> GetVigentePorPropietario()
           {
               try
               {
                   var propietario = await _context.Propietarios.Where(propietario => propietario.Email == User.Identity.Name).FirstOrDefaultAsync();
                   var inmuebles = await _context.Contratos
                  .Include(contrato => contrato.Inmueble)
                  .Where(contrato => contrato.FechaInicio <= DateTime.Now &&
                         contrato.FechaFin >= DateTime.Now &&
                         contrato.Inmueble.IdPropietario == propietario.Id)
                  .Select(contrato => contrato.Inmueble).ToListAsync();
                   if (inmuebles.Count > 0)
                   {
                       return Ok(inmuebles);
                   }
                   else return NotFound("No existen inmuebles ocupados en este momento");
               }
               catch (Exception ex)
               {
                   return BadRequest(ex);
               }

           }
           // GET: api/Inmuebles/5
           [HttpGet("{id}")]
           public async Task<ActionResult<Inmueble>> GetInmueble(int id)
           {
               var inmueble = await _context.Inmuebles.FindAsync(id);

               if (inmueble == null)
               {
                   return NotFound();
               }

               return inmueble;
           }


           // PUT: api/Inmuebles/5
           // To protect from overposting attacks, enable the specific properties you want to bind to, for
           // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
           [HttpPut("{id}")]
           public async Task<IActionResult> PutInmueble(int id, Inmueble inmueble)
           {
               if (id != inmueble.Id)
               {
                   return BadRequest();
               }

               _context.Entry(inmueble).State = EntityState.Modified;

               try
               {
                   await _context.SaveChangesAsync();
               }
               catch (DbUpdateConcurrencyException)
               {
                   if (!InmuebleExists(id))
                   {
                       return NotFound();
                   }
                   else
                   {
                       throw;
                   }
               }

               return NoContent();
           }
           [HttpPut]
           public async Task<ActionResult<Inmueble>> PutInmueble(Inmueble inmueble)
           {
               try
               {
                   var inm = await _context.Inmuebles.Where(i => i.Id == inmueble.Id).FirstOrDefaultAsync();
                   var propietario = await _context.Propietarios.Where(propietario => propietario.Email == User.Identity.Name).FirstOrDefaultAsync();
                   if(inm.IdPropietario == propietario.Id)
                   {
                       _context.Inmuebles.Update(inmueble);
                       _context.SaveChanges();
                       return Ok(inmueble);
                   }
                   else
                   {
                       return BadRequest();
                   }   
               }
               catch (Exception ex)
               {
                   return BadRequest(ex);
               }
           }
           // POST: api/Inmuebles
           // To protect from overposting attacks, enable the specific properties you want to bind to, for
           // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
           [HttpPost]
           public async Task<ActionResult<Inmueble>> PostInmueble(Inmueble inmueble)
           {
               _context.Inmuebles.Add(inmueble);

               await _context.SaveChangesAsync();

               return CreatedAtAction("GetInmueble", new { id = inmueble.Id }, inmueble);
           }
           [HttpPost("Foto")]
           public async Task<ActionResult<Inmueble>> PostFotoInmueble([FromForm] IFormFile file)
           {
               if (file != null)
               {
                   string root = enviroment.WebRootPath;
                   string path = Path.Combine(root, "Uploads");
                   if (!Directory.Exists(path))
                   {
                       Directory.CreateDirectory(path);

                   }
                   string fileName = file.FileName + Path.GetExtension(file.FileName);
                   string pathCompleto = Path.Combine(path, fileName);
                   using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                   {
                       file.CopyTo(stream);
                   }
               }
               return Ok("Foto subida satisfactoriamente");
           }

           // DELETE: api/Inmuebles/5
           [HttpDelete("{id}")]
           public async Task<ActionResult<Inmueble>> DeleteInmueble(int id)
           {
               var inmueble = await _context.Inmuebles.FindAsync(id);
               if (inmueble == null)
               {
                   return NotFound();
               }

               _context.Inmuebles.Remove(inmueble);
               await _context.SaveChangesAsync();

               return inmueble;
           }

           private bool InmuebleExists(int id)
           {
               return _context.Inmuebles.Any(e => e.Id == id);
           }
       }*/
    }
    }