using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{
    public class Propietario
    {
        private int id;
        private string nombre;
        private string dni;
        private string apellido;
        private string email;
        private string telefono;
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
        [Required, DataType(DataType.Password)]
        public string Contraseña { get; set; }
        [Required]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }


    }
}
