using System.Collections.Generic;

namespace Inmobiliaria.Models
{
    public interface IRepositorio<T>
    {
        public int Create(T entidad);
        public int Edit(int id, T entidad);
        public int Delete(int id);
        public T ObtenerPorId(int id);
        public List<T> ObtenerTodos();

    }
}
