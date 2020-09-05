using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inmobiliaria.Models
{
    public class RepositorioContrato
    {
        IConfiguration configuration;
        string connectionString;
        public RepositorioContrato(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = this.configuration["ConnectionStrings:DefaultConnection"];
        }

        public int Create(Contrato contrato)
        {
            int resultado = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Contratos(FechaInicio, FechaFin, MontoAlquiler, IdInquilino, IdInmueble) " +
                    "VALUES (@fechaInicio, @fechaFin, @montoAlquiler, @idInquilino, @idInmueble);" +
                    "SELECT SCOPE_IDENTITY()";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                    command.Parameters.AddWithValue("@montoAlquiler", contrato.MontoAlquiler);
                    command.Parameters.AddWithValue("@idInquilino", contrato.IdInquilino);
                    command.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble);
                    connection.Open();
                    try
                    {
                        resultado = Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    contrato.IdContrato = resultado;
                    connection.Close();

                }
            }

            return resultado;
        }
        public int Edit(int id, Contrato contrato)
        {
            int resultado = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Contratos SET FechaInicio = @fechaInicio, FechaFin = @fechaFin, MontoAlquiler = @montoAlquiler, " +
                    "IdInquilino = @idInquilino, IdInmueble = @idInmuele WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fechaInicio", contrato.FechaInicio);
                    command.Parameters.AddWithValue("@fechaFin", contrato.FechaFin);
                    command.Parameters.AddWithValue("@montoAlquiler", contrato.MontoAlquiler);
                    command.Parameters.AddWithValue("@idInquilino", contrato.IdInquilino);
                    command.Parameters.AddWithValue("@idInmueble", contrato.IdInmueble);
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
        public int Delete(int id)
        {
            int resultado = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "DELETE FROM Contratos WHERE id = @id";
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
        public Contrato ObtenerPorId(int id)
        {
            Contrato contrato = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT c.Id, FechaInicio, FechaFin, MontoAlquiler, IdInquilino, IdInmueble," +
                    " DNI, Nombre, Apellido, Direccion " +
                    "from Contratos c INNER JOIN Inquilinos i ON c.IdInquilino = i.Id " +
                    "INNER JOIN Inmuebles inm ON c.IdInmueble = inm.Id WHERE c.Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    connection.Open();
                    try
                    {
                        var reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            contrato = new Contrato
                            {
                                IdContrato = reader.GetInt32(0),
                                FechaInicio = reader.GetDateTime(1),
                                FechaFin = reader.GetDateTime(2),
                                MontoAlquiler = reader.GetDecimal(3),
                                IdInquilino = reader.GetInt32(4),
                                IdInmueble = reader.GetInt32(5),
                                Inquilino = new Inquilino
                                {
                                    IdInquilino = reader.GetInt32(4),
                                    DNI = reader.GetString(6),
                                    Nombre = reader.GetString(7),
                                    Apellido = reader.GetString(8),
                                },
                                Inmueble = new Inmueble
                                {
                                    IdInmueble = reader.GetInt32(5),
                                    Direccion = reader.GetString(9),
                                }
                            };

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    connection.Close();
                }
                return contrato;
            }
        }



        public List<Contrato> ObtenerTodos()
        {
            List<Contrato> contratos = new List<Contrato>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT c.Id, FechaInicio, FechaFin, MontoAlquiler, IdInquilino, IdInmueble," +
                    " DNI, Nombre, Apellido, Direccion, Tipo " +
                    "from Contratos c INNER JOIN Inquilinos i ON c.IdInquilino = i.Id " +
                    "INNER JOIN Inmuebles inm ON c.IdInmueble = inm.Id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    try
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Contrato contrato = new Contrato
                            {
                                IdContrato = reader.GetInt32(0),
                                FechaInicio = reader.GetDateTime(1),
                                FechaFin = reader.GetDateTime(2),
                                MontoAlquiler = reader.GetDecimal(3),
                                IdInquilino = reader.GetInt32(4),
                                IdInmueble = reader.GetInt32(5),
                                Inquilino = new Inquilino
                                {
                                    IdInquilino = reader.GetInt32(4),
                                    DNI = reader.GetString(6),
                                    Nombre = reader.GetString(7),
                                    Apellido = reader.GetString(8),
                                },
                                Inmueble = new Inmueble
                                {
                                    IdInmueble = reader.GetInt32(5),
                                    Direccion = reader.GetString(9),
                                    Tipo = reader.GetString(10),
                                }
                            };

                            contratos.Add(contrato);
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    connection.Close();

                }
            }
            return contratos;
        }
    }
}
