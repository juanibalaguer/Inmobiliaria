using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Inmobiliaria.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InmueblesController : ControllerBase
    {
        private readonly DataContext _context;

        public InmueblesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Inmuebles/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inmueble>>> GetPorPropietario()
        {
            try
            {
                var propietario = await _context.Propietarios.Where(propietario => propietario.Email == User.Identity.Name).FirstOrDefaultAsync();
                var inmuebles = await _context.Inmuebles.Where(inmueble => inmueble.IdPropietario == propietario.Id).ToListAsync();
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
        // GET: api/Inmuebles/
        [HttpGet("Vigentes/")]
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
    }
}
