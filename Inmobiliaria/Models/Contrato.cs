using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int Id { get; set; }
        [Required]
        [Display(Name = "Fecha de inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }
        [Required]
        [Display(Name = "Fecha de finalización")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }
        [Required]
        [Display(Name = "Alquiler mensual")]
        public decimal MontoAlquiler { get; set; }
        [Required]
        [ForeignKey("Inquilino")]
        [Display(Name = "Inquilino")]
        public int IdInquilino { get; set; }
        [Display(Name = "Inmueble")]
        [ForeignKey("Inmueble")]
        [Required]
        public int IdInmueble { get; set; }
        [Required]
        public Inquilino Inquilino { get; set; }
        [Required]
        public Inmueble Inmueble { get; set; }

    }
}
