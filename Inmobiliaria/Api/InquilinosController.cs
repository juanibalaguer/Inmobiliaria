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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class InquilinosController : ControllerBase
    {
        private readonly DataContext _context;

        public InquilinosController(DataContext context)
        {
            _context = context;
        }


        // GET: api/Inquilinos
        [HttpGet("{id}")]
        public async Task<ActionResult<Inquilino>> GetInquilino(int id)
        {
            try
            {
                var propietario = await _context.Propietarios
                    .Where(propietario => propietario.Email == User.Identity.Name).FirstOrDefaultAsync();
                var inquilino = await _context.Contratos
                       .Include(contrato => contrato.Inmueble)
                       .Include(contrato => contrato.Inquilino)
                       .Where(contrato => contrato.IdInmueble == id &&
                       contrato.Inmueble.IdPropietario == propietario.Id &&
                       DateTime.Now >= contrato.FechaInicio && DateTime.Now <= contrato.FechaFin)
                       .Select(contrato => contrato.Inquilino)
                       .FirstOrDefaultAsync();
                if (inquilino == null)
                {
                    return NotFound("No se encontró inquilino");
                }

                return Ok(inquilino);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        

        private bool InquilinoExists(int id)
        {
            return _context.Inquilinos.Any(e => e.Id == id);
        }
    }
}
