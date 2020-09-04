using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class Inmueble
    {
        private int idInmueble;
        private string direccion;
        private string uso;
        private string tipo;
        private int ambientes;
        private decimal precio;
        private int idPropietario;
        private Propietario propietario;

        [Key]
        [Display(Name="Código")]
        public int IdInmueble { get; set; }
        [Required]
        public string Direccion { get; set;}
        [Required]
        public string Uso { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public int Ambientes { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        [Display(Name ="Propietario")]
        public int IdPropietario { get; set; }
        [Required]
        public Propietario Propietario { get; set; }


    }
}
