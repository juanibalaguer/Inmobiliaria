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
        public int IdPropietario { get; set;}
        public int DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }


    }
}
