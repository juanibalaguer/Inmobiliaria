using Microsoft.Extensions.Configuration;

namespace Inmobiliaria.Models
{
    public abstract class Repositorio
    {
        protected readonly string connectionString;
        protected readonly IConfiguration configuration;
        public Repositorio(IConfiguration iconfiguration)
        {
            this.configuration = iconfiguration;
            this.connectionString = this.configuration["ConnectionStrings:DefaultConnection"];

        }
    }
}
