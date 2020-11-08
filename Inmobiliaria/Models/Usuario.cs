using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inmobiliaria.Models
{
    public enum roles
    {
        Administrador = 1,
        Empleado = 2,
    }
    public class Usuario
    {

        private int idUsuario;
        private string email;
        private string nombre;
        private string apellido;
        private string contraseña;
        private int rol;



        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }
        public int Rol { get; set; }
        public string AvatarUrl { get; set; }
        [Display(Name = "Avatar")]
        public IFormFile AvatarFile { get; set; }

        public string NombreRol()
        {
            return Rol > 0 ? ((roles)Rol).ToString() : "";
        }
        public static IDictionary<int, string> ObtenerRoles()
        {
            SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
            Type tipoRol = typeof(roles);
            foreach (var valor in Enum.GetValues(tipoRol))
            {
                roles.Add((int)valor, Enum.GetName(tipoRol, valor));
            }
            return roles;
        }
    }
}
