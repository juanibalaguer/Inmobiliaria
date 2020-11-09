using Inmobiliaria.Models;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inmobiliaria.Api
{

    //Propietarios de los contratos vigentes
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PropietariosController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration iconfiguration;

        public PropietariosController(DataContext context, IConfiguration iconfiguration)
        {
            this.iconfiguration = iconfiguration;
            _context = context;
        }
        // GET api/<controller>/5
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginView login)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: login.Contraseña,
                    salt: System.Text.Encoding.ASCII.GetBytes(iconfiguration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));
                var propietario = _context.Propietarios
                    .Where(propietario => propietario.Email == login.Usuario)
                    .FirstOrDefault();
                if (propietario != null && hashed == propietario.Contraseña.TrimEnd())
                {
                    var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(iconfiguration["TokenAuthentication:SecretKey"]));
                    var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, propietario.Email),
                        new Claim("FullName", propietario.Nombre + " " + propietario.Apellido),
                        new Claim(ClaimTypes.Role, "Propietario"),
                    };

                    var token = new JwtSecurityToken(
                        issuer: iconfiguration["TokenAuthentication:Issuer"],
                        audience: iconfiguration["TokenAuthentication:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(60),
                        signingCredentials: credenciales
                    );
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }

                else
                {
                    return BadRequest("Nombre de usuario o clave incorrecta");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET: api/Propietarios
        [HttpGet("GetPropietariosVigentes")]
        public async Task<ActionResult<IEnumerable<Propietario>>> GetPropietariosVigentes()
        {
            var propietarios = await _context.Contratos
                .Include(contrato => contrato.Inmueble)
                .ThenInclude(inmueble => inmueble.Propietario)
                .Where(contrato => DateTime.Now >= contrato.FechaInicio && contrato.FechaFin >= DateTime.Now)
                .Select(contrato => contrato.Inmueble.Propietario).ToListAsync();

            return Ok(propietarios);
        }

        // GET: api/Propietarios/
        [HttpGet]
        public async Task<ActionResult<Propietario>> GetPropietario()
        {
            try
            {
                var propietario = await _context.Propietarios
                    .Where(propietario => propietario.Email == User.Identity.Name)
                    .FirstOrDefaultAsync();
                return Ok(propietario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // PUT: api/Propietarios/
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        public async Task<IActionResult> PutPropietario(Propietario propietario)
        {
            try
            {
                if (_context.Propietarios.AsNoTracking().FirstOrDefault(propietario => propietario.Email == User.Identity.Name) != null)
                { 
                    _context.Entry(propietario).State = EntityState.Modified;
                    _context.Entry(propietario).Property(propietario => propietario.Contraseña).IsModified = false;
                    await _context.SaveChangesAsync();
                    return Ok(propietario);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        private bool PropietarioExists(int id)
        {
            return _context.Propietarios.Any(e => e.Id == id);
        }
    }
}
