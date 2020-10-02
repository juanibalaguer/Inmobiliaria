using System;
using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{
    public class Pago
    {
        private int idPago;
        private int numero;
        private int idContrato;
        private Contrato contrato;
        private decimal importe;
        private DateTime fechaDePago;


        [Key]
        [Display(Name = "Código")]
        public int IdPago { get; set; }
        [Display(Name = "Número")]
        public int Numero { get; set; }
        [Required]
        [Display(Name = "Contrato")]
        public int IdContrato { get; set; }
        [Required]
        public Contrato Contrato { get; set; }
        [Required]
        public decimal Importe { get; set; }
        [Required]
        [Display(Name = "Fecha de pago")]
        [DataType(DataType.Date)]
        public DateTime FechaDePago { get; set; }

    }
}
