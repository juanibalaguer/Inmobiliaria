using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class Inquilino
    {
        private int idInquilino;
        private string nombre;
        private int dni;
        private string apellido;
        private string lugarDeTrabajo;
        private string email;
        private string telefono;
        private string nombreGarante;
        private string telefonoGarante;


        [Key]
        [Display(Name = "Código")]
        public int IdInquilino { get; set; }
        public int DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }
        [Display(Name = "Nombre garante")]
        public string NombreGarante { get; set; }
        [Display(Name = "Teléfono garante")]
        public string TelefonoGarante { get; set; }
    }
}
