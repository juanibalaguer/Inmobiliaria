using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class Propietario
    {
        private int idPropietario;
        private string nombre;
        private int dni;
        private string apellido;
        private string email;
        private string telefono;
        [Key]
        [Display(Name = "Código")]
        public int IdPropietario { get; set; }
        [Required]
        public int DNI { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }


    }
}
