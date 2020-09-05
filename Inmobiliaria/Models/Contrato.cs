using System;
using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{
    public class Contrato
    {
        private int idContrato;
        private DateTime fechaInicio;
        private DateTime fechaFin;
        private decimal montoAlquiler;
        private int idInquilino;
        private int idInmueble;
        private Inquilino inquilino;
        private Inmueble inmueble;

        [Key]
        [Display(Name = "Código")]
        public int IdContrato { get; set; }
        [Required]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }
        [Required]
        [Display(Name = "Fecha de finalización")]
        public DateTime FechaFin { get; set; }
        [Required]
        [Display(Name = "Alquiler mensual")]
        public decimal MontoAlquiler { get; set; }
        [Required]
        [Display(Name = "Inquilino")]
        public int IdInquilino { get; set; }
        [Display(Name = "Inmueble")]
        [Required]
        public int IdInmueble { get; set; }
        [Required]
        public Inquilino Inquilino { get; set; }
        [Required]
        public Inmueble Inmueble { get; set; }

    }
}
