﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inmobiliaria.Models
{
    public class RepositorioInquilino : Repositorio, IRepositorio<Inquilino>
    {

        public RepositorioInquilino(IConfiguration iconfiguration) : base(iconfiguration)
        {

        }

        public int Create(Inquilino inquilino)
        {
            int resultado = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Inquilinos(DNI, Nombre, Apellido, Email, Telefono, NombreGarante, TelefonoGarante)" +
                    "VALUES (@DNI, @nombre, @apellido, @email, @telefono, @nombreGarante, @telefonoGarante);" +
                    "SELECT SCOPE_IDENTITY()";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DNI", inquilino.DNI);
                    command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
                    command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
                    command.Parameters.AddWithValue("@email", inquilino.Email);
                    command.Parameters.AddWithValue("@telefono", inquilino.Telefono);
                    command.Parameters.AddWithValue("@nombreGarante", inquilino.NombreGarante);
                    command.Parameters.AddWithValue("@telefonoGarante", inquilino.TelefonoGarante);
                    connection.Open();
                    try
                    {
                        resultado = Convert.ToInt32(command.ExecuteScalar());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    inquilino.Id = resultado;
                    connection.Close();

                }
            }

            return resultado;
        }
        public int Edit(int id, Inquilino inquilino)
        {
            int resultado = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "UPDATE Inquilinos SET DNI = @DNI, Nombre = @nombre, Apellido = @apellido," +
                    "Email = @email, Telefono = @telefono, NombreGarante=@nombreGarante, TelefonoGarante=@telefonoGarante" +
                    " WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DNI", inquilino.DNI);
                    command.Parameters.AddWithValue("@nombre", inquilino.Nombre);
                    command.Parameters.AddWithValue("@apellido", inquilino.Apellido);
                    command.Parameters.AddWithValue("@email", inquilino.Email);
                    command.Parameters.AddWithValue("@telefono", inquilino.Telefono);
                    command.Parameters.AddWithValue("@nombreGarante", inquilino.NombreGarante);
                    command.Parameters.AddWithValue("@telefonoGarante", inquilino.TelefonoGarante);
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
                string sql = "DELETE FROM Inquilinos WHERE id = @id";
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
        public Inquilino ObtenerPorId(int id)
        {
            Inquilino inquilino = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Id, DNI, Nombre, Apellido, Email, Telefono, NombreGarante, TelefonoGarante " +
                "from Inquilinos WHERE id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    connection.Open();
                    try
                    {
                        var reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(0),
                                DNI = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Apellido = reader.GetString(3),
                                Email = reader.GetString(4),
                                Telefono = reader.GetString(5),
                                NombreGarante = reader.GetString(6),
                                TelefonoGarante = reader.GetString(7),
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
            return inquilino;
        }

        public List<Inquilino> ObtenerTodos()
        {
            List<Inquilino> inquilinos = new List<Inquilino>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Id, DNI, Nombre, Apellido, Email, Telefono, NombreGarante, TelefonoGarante " +
                "FROM Inquilinos ORDER BY  Apellido, Nombre";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    try
                    {
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Inquilino inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(0),
                                DNI = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Apellido = reader.GetString(3),
                                Email = reader.GetString(4),
                                Telefono = reader.GetString(5),
                                NombreGarante = reader.GetString(6),
                                TelefonoGarante = reader.GetString(7),
                            };

                            inquilinos.Add(inquilino);
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    connection.Close();

                }
            }
            return inquilinos;
        }
        public List<Inquilino> ObtenerTodosPorPagina(int nroPagina)
        {
            List<Inquilino> inquilinos = new List<Inquilino>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Id, DNI, Nombre, Apellido, Email, Telefono, NombreGarante, TelefonoGarante " +
                "FROM Inquilinos ORDER BY  Apellido, Nombre OFFSET @nroPagina ROWS FETCH FIRST 1 ROWS ONLY";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    try
                    {
                        command.Parameters.Add("@nroPagina", SqlDbType.Int).Value = nroPagina;
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Inquilino inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(0),
                                DNI = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Apellido = reader.GetString(3),
                                Email = reader.GetString(4),
                                Telefono = reader.GetString(5),
                                NombreGarante = reader.GetString(6),
                                TelefonoGarante = reader.GetString(7),
                            };

                            inquilinos.Add(inquilino);
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    connection.Close();

                }
            }
            return inquilinos;
        }
    }

}
