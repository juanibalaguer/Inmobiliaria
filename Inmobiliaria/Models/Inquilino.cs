using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{
    public class Inquilino
    {
        private int idInquilino;
        private string nombre;
        private string dni;
        private string apellido;
        private string lugarDeTrabajo;
        private string email;
        private string telefono;
        private string nombreGarante;
        private string telefonoGarante;


        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        public string DNI { get; set; }
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
        [Required]
        [Display(Name = "Nombre garante")]
        public string NombreGarante { get; set; }
        [Required]
        [Display(Name = "Teléfono garante")]
        public string TelefonoGarante { get; set; }
    }
}
