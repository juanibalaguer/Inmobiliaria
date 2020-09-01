using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class RepositorioPropietario
    {
        IConfiguration configuration;
        string connectionString;
        public RepositorioPropietario(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = this.configuration["ConnectionStrings:DefaultConnection"];
        }

        public int Create(Propietario propietario)
        {
            int resultado = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Propietarios(DNI, Nombre, Apellido, Email, Telefono)" +
                    "VALUES (@DNI, @nombre, @apellido, @email, @telefono);" +
                    "SELECT SCOPE_IDENTITY()";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DNI", propietario.DNI);
                    command.Parameters.AddWithValue("@nombre", propietario.Nombre);
                    command.Parameters.AddWithValue("@apellido", propietario.Apellido);
                    command.Parameters.AddWithValue("@email", propietario.Email);
                    command.Parameters.AddWithValue("@telefono", propietario.Telefono);
                    connection.Open();
                    try
                    {
                        resultado = Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    propietario.IdPropietario = resultado;
                    connection.Close();

                }
            }

            return resultado;
        }
        public int Edit(int id, Propietario propietario)
        {
            int resultado = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Propietarios SET DNI = @DNI, Nombre = @nombre, Apellido = @apellido," +
                    "Email = @email, Telefono = @telefono WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DNI", propietario.DNI);
                    command.Parameters.AddWithValue("@nombre", propietario.Nombre);
                    command.Parameters.AddWithValue("@apellido", propietario.Apellido);
                    command.Parameters.AddWithValue("@email", propietario.Email);
                    command.Parameters.AddWithValue("@telefono", propietario.Telefono);
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    try
                    {
                        resultado = command.ExecuteNonQuery();
                    } catch (Exception e)
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
                string sql = "DELETE FROM Propietarios WHERE id = @id";
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
        public Propietario ObtenerPorId(int id)
        {
            Propietario propietario = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Id, DNI, Nombre, Apellido, Email, Telefono " +
                    "from Propietarios WHERE id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    connection.Open();
                    try
                    {
                        var reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            propietario = new Propietario
                            {
                                IdPropietario = reader.GetInt32(0),
                                DNI = reader.GetInt32(1),
                                Nombre = reader.GetString(2),
                                Apellido = reader.GetString(3),
                                Email = reader.GetString(4),
                                Telefono = reader.GetString(5),
                            };
                        }   
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    
                    connection.Close();
                }
            }
            return propietario;
        }

        public List<Propietario> ObtenerTodos()
        {
            List<Propietario> propietarios = new List<Propietario>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Id, DNI, Nombre, Apellido, Email, Telefono " +
                    "from Propietarios";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    try
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Propietario propietario = new Propietario
                            {
                                IdPropietario = reader.GetInt32(0),
                                DNI = reader.GetInt32(1),
                                Nombre = reader.GetString(2),
                                Apellido = reader.GetString(3),
                                Email = reader.GetString(4),
                                Telefono = reader.GetString(5),
                            };

                            propietarios.Add(propietario);
                        }
                    } catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    
                    connection.Close();

                }
            }
            return propietarios;
        }
    }
}
    


