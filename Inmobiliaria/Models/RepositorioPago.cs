using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inmobiliaria.Models
{
    public class RepositorioPago : Repositorio, IRepositorio<Pago>
    {

        public RepositorioPago(IConfiguration iconfiguration) : base(iconfiguration)
        {

        }

        public int Create(Pago pago)
        {
            int resultado = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Pagos(Numero, IdContrato, Importe, FechaDePago) " +
                    "VALUES (@numero, @idContrato, @importe, @fechaDePago);" +
                    "SELECT SCOPE_IDENTITY()";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@numero", pago.Numero);
                    command.Parameters.AddWithValue("@idContrato", pago.IdContrato);
                    command.Parameters.AddWithValue("@importe", pago.Importe);
                    command.Parameters.AddWithValue("@fechaDePago", pago.FechaDePago);
                    connection.Open();
                    try
                    {
                        resultado = Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    pago.IdPago = resultado;
                    connection.Close();

                }
            }

            return resultado;
        }
        public int Edit(int id, Pago pago)
        {
            int resultado = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Pagos SET Numero = @numero, IdContrato = @idContrato, Importe = @importe, " +
                    "FechaDePago = @fechaDePago WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@numero", pago.Numero);
                    command.Parameters.AddWithValue("@idContrato", pago.IdContrato);
                    command.Parameters.AddWithValue("@importe", pago.Importe);
                    command.Parameters.AddWithValue("@fechaDePago", pago.FechaDePago);
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
                string sql = "DELETE FROM Pagos WHERE id = @id";
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
        public Pago ObtenerPorId(int id)
        {
            Pago pago = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Pagos.Id, Numero, IdContrato, Importe, FechaDePago, MontoAlquiler " +
                    "FROM Pagos INNER JOIN Contratos on IdContrato = Contratos.Id WHERE Pagos.Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    connection.Open();
                    try
                    {
                        var reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            pago = new Pago
                            {
                                IdPago = reader.GetInt32(0),
                                Numero = reader.GetInt32(1),
                                IdContrato = reader.GetInt32(2),
                                Importe = reader.GetDecimal(3),
                                FechaDePago = reader.GetDateTime(4),
                                Contrato = new Contrato
                                {
                                    MontoAlquiler = reader.GetDecimal(5),
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
                return pago;
            }
        }


        public List<Pago> ObtenerTodos()
        {
            return ObtenerTodos(0);
        }
        public List<Pago> ObtenerTodos(int idContrato)
        {
            List<Pago> pagos = new List<Pago>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Pagos.Id, Numero, IdContrato, Importe, FechaDePago, MontoAlquiler " +
                    "FROM Pagos INNER JOIN Contratos on IdContrato = Contratos.Id ";
                if (idContrato > 0)
                {
                    sql += "WHERE IdContrato = @idContrato";
                }
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    try
                    {
                        if (idContrato > 0)
                        {
                            command.Parameters.AddWithValue("@idContrato", idContrato);
                        }
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Pago pago = new Pago
                            {
                                IdPago = reader.GetInt32(0),
                                Numero = reader.GetInt32(1),
                                IdContrato = reader.GetInt32(2),
                                Importe = reader.GetDecimal(3),
                                FechaDePago = reader.GetDateTime(4),
                                Contrato = new Contrato
                                {
                                    MontoAlquiler = reader.GetDecimal(5),
                                }

                            };

                            pagos.Add(pago);
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    connection.Close();

                }
            }
            return pagos;
        }
        public List<Pago> ObtenerPorContrato(int idContrato)
        {
            List<Pago> pagos = new List<Pago>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Pagos.Id, Numero, IdContrato, Importe, FechaDePago, MontoAlquiler " +
                    "FROM Pagos INNER JOIN Contratos on IdContrato = Contratos.Id WHERE IdContrato = @idContrato";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    try
                    {
                        command.Parameters.Add("@idContrato", SqlDbType.Int).Value = idContrato;
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Pago pago = new Pago
                            {
                                IdPago = reader.GetInt32(0),
                                Numero = reader.GetInt32(1),
                                IdContrato = reader.GetInt32(2),
                                Importe = reader.GetDecimal(3),
                                FechaDePago = reader.GetDateTime(4),
                                Contrato = new Contrato
                                {
                                    MontoAlquiler = reader.GetDecimal(5),
                                }

                            };

                            pagos.Add(pago);
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    connection.Close();

                }
            }
            return pagos;
        }
    }
}
