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
    public class PagosController : ControllerBase
    {
        private readonly DataContext _context;

        public PagosController(DataContext context)
        {
            _context = context;
        }
        // GET: api/Pagos
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Pago>>> GetPagosVigentesPorPropietario(int id)
        {
            try
            {
                var propietario = await _context.Propietarios.Where(propietario => propietario.Email == User.Identity.Name).FirstOrDefaultAsync();
                var pagos = await _context.Pagos
                .Include(pago => pago.Contrato)
                .ThenInclude(contrato => contrato.Inmueble)
                .Where(pago => pago.Contrato.FechaInicio <= DateTime.Now && pago.Contrato.FechaFin >= DateTime.Now
                        && pago.Contrato.Inmueble.IdPropietario == propietario.Id
                        && pago.Contrato.IdInmueble == id)
                .ToListAsync();
                if (pagos.Count > 0)
                {
                    return Ok(pagos);
                }
                else return NotFound("No existen pagos");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            
        }
        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.Id == id);
        }
    }
}
