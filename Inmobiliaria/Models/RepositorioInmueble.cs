using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inmobiliaria.Models
{
    public class RepositorioInmueble : Repositorio, IRepositorio<Inmueble>
    {
        public RepositorioInmueble(IConfiguration iconfiguration) : base(iconfiguration)
        {

        }

        public int Create(Inmueble inmueble)
        {
            int resultado = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Inmuebles(Direccion, Uso, Tipo, Ambientes, Precio," +
                    "IdPropietario, Estado) VALUES (@direccion, @uso, @tipo, @ambientes, @precio,@idPropietario, @estado);" +
                    "SELECT SCOPE_IDENTITY()";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
                    command.Parameters.AddWithValue("@uso", inmueble.Uso);
                    command.Parameters.AddWithValue("@tipo", inmueble.Tipo);
                    command.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
                    command.Parameters.AddWithValue("@precio", inmueble.Precio);
                    command.Parameters.AddWithValue("@idPropietario", inmueble.IdPropietario);
                    command.Parameters.AddWithValue("@estado", inmueble.Estado);
                    connection.Open();
                    try
                    {
                        resultado = Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    inmueble.IdInmueble = resultado;
                    connection.Close();

                }
            }

            return resultado;
        }
        public int Edit(int id, Inmueble inmueble)
        {
            int resultado = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Inmuebles SET Direccion = @direccion, Uso = @uso, Tipo = @tipo, " +
                    "Ambientes = @ambientes, Precio = @precio, IdPropietario = @idPropietario, Estado = @estado WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@direccion", inmueble.Direccion);
                    command.Parameters.AddWithValue("@uso", inmueble.Uso);
                    command.Parameters.AddWithValue("@tipo", inmueble.Tipo);
                    command.Parameters.AddWithValue("@ambientes", inmueble.Ambientes);
                    command.Parameters.AddWithValue("@precio", inmueble.Precio);
                    command.Parameters.AddWithValue("@idPropietario", inmueble.IdPropietario);
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@estado", inmueble.Estado);
                    connection.Open();
                    try
                    {
                        resultado = command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    connection.Close();

                }
            }

            return resultado;
        }
        public int Delete(int id)
        {
            int resultado = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM Inmuebles WHERE id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    try
                    {
                        resultado = command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    connection.Close();
                }
            }
            return resultado;
        }
        public Inmueble ObtenerPorId(int id)
        {
            Inmueble inmueble = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT i.Id,Direccion, Uso, Tipo, Ambientes, Precio, IdPropietario, DNI, p.Nombre," +
                    " p.Apellido, Estado from Inmuebles i INNER JOIN Propietarios p ON i.IdPropietario = p.Id WHERE i.Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    connection.Open();
                    try
                    {
                        var reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(0),
                                Direccion = reader.GetString(1),
                                Uso = reader.GetString(2),
                                Tipo = reader.GetString(3),
                                Ambientes = reader.GetInt32(4),
                                Precio = reader.GetDecimal(5),
                                IdPropietario = reader.GetInt32(6),
                                Propietario = new Propietario
                                {
                                    IdPropietario = reader.GetInt32(6),
                                    DNI = reader.GetString(7),
                                    Nombre = reader.GetString(8),
                                    Apellido = reader.GetString(9),
                                },
                                Estado = reader.GetByte(10) == 1 ? true : false,
                            };

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    connection.Close();
                }
                return inmueble;
            }
        }



        public List<Inmueble> ObtenerTodos()
        {
            List<Inmueble> inmuebles = new List<Inmueble>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT i.Id,Direccion, Uso, Tipo, Ambientes, Precio, IdPropietario, DNI, p.Nombre," +
                    " p.Apellido, Estado from Inmuebles i INNER JOIN Propietarios p ON i.IdPropietario = p.Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    try
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Inmueble inmueble = new Inmueble
                            {
                                IdInmueble = reader.GetInt32(0),
                                Direccion = reader.GetString(1),
                                Uso = reader.GetString(2),
                                Tipo = reader.GetString(3),
                                Ambientes = reader.GetInt32(4),
                                Precio = reader.GetDecimal(5),
                                IdPropietario = reader.GetInt32(6),
                                Propietario = new Propietario
                                {
                                    IdPropietario = reader.GetInt32(6),
                                    DNI = reader.GetString(7),
                                    Nombre = reader.GetString(8),
                                    Apellido = reader.GetString(9),
                                },
                                Estado = reader.GetByte(10) == 1 ? true : false,
                            };

                            inmuebles.Add(inmueble);
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    connection.Close();

                }
            }
            return inmuebles;
        }
    }
}

