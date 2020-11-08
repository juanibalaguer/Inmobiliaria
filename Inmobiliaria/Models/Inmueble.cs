using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        private bool estado;
        private string foto;

        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public string Uso { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public int Ambientes { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Precio { get; set; }
        [Required]
        [Display(Name = "Propietario")]
        [ForeignKey("Propietario")]
        public int IdPropietario { get; set; }
        [Required]
        public Propietario Propietario { get; set; }
        [Required]
        public bool Estado { get; set; }
        public string Foto { get; set; }


    }
}
