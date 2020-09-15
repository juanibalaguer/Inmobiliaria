using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria.Models
{
    public class RepositorioPago
    {
        IConfiguration configuration;
        string connectionString;
        public RepositorioPago(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.connectionString = this.configuration["ConnectionStrings:DefaultConnection"];
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
                string sql = "SELECT Id, Numero, IdContrato, Importe, FechaDePago FROM Pagos WHERE Id = @id";
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
            List<Pago> pagos = new List<Pago>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Id, Numero, IdContrato, Importe, FechaDePago FROM Pagos";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    try
                    {
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
