﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inmobiliaria.Models
{
    public class RepositorioPropietario : Repositorio, IRepositorio<Propietario>
    {

        int itemsPorPagina;
        public RepositorioPropietario(IConfiguration iconfiguration) : base(iconfiguration)
        {
            this.itemsPorPagina = Convert.ToInt32(this.configuration["ItemsPorPagina"]);
        }

        public int Create(Propietario propietario)
        {
            int resultado = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO Propietarios(DNI, Nombre, Apellido, Email, Contraseña, Telefono)" +
                    "VALUES (@DNI, @nombre, @apellido, @email, @contraseña, @telefono);" +
                    "SELECT SCOPE_IDENTITY()";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DNI", propietario.DNI);
                    command.Parameters.AddWithValue("@nombre", propietario.Nombre);
                    command.Parameters.AddWithValue("@apellido", propietario.Apellido);
                    command.Parameters.AddWithValue("@email", propietario.Email);
                    command.Parameters.AddWithValue("@contraseña", propietario.Contraseña);
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

                    propietario.Id = resultado;
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
                    "Email = @email, Contraseña = @contraseña, Telefono = @telefono WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@DNI", propietario.DNI);
                    command.Parameters.AddWithValue("@nombre", propietario.Nombre);
                    command.Parameters.AddWithValue("@apellido", propietario.Apellido);
                    command.Parameters.AddWithValue("@email", propietario.Email);
                    command.Parameters.AddWithValue("@contraseña", propietario.Contraseña);
                    command.Parameters.AddWithValue("@telefono", propietario.Telefono);
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
                string sql = "SELECT Id, DNI, Nombre, Apellido, Email, Contraseña, Telefono " +
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
                                Id = reader.GetInt32(0),
                                DNI = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Apellido = reader.GetString(3),
                                Email = reader.GetString(4),
                                Contraseña = reader.GetString(5),
                                Telefono = reader.GetString(6),
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
                string sql = "SELECT Id, DNI, Nombre, Apellido, Email, Contraseña, Telefono " +
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
                                Id = reader.GetInt32(0),
                                DNI = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Apellido = reader.GetString(3),
                                Email = reader.GetString(4),
                                Contraseña = reader.GetString(5),
                                Telefono = reader.GetString(6),
                            };

                            propietarios.Add(propietario);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    connection.Close();

                }
            }
            return propietarios;
        }
        public int ContarPropietarios()
        {
            int nroPropietarios = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(*) from Propietarios";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    try
                    {
                        nroPropietarios = Convert.ToInt32(command.ExecuteScalar());

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    connection.Close();

                }
            }
            return nroPropietarios;
        }
        public List<Propietario> ObtenerTodosPorPagina(int nroPagina)
        {
            List<Propietario> propietarios = new List<Propietario>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT Id, DNI, Nombre, Apellido, Email, Contraseña, Telefono " +
                    "from Propietarios ORDER BY  Apellido, Nombre OFFSET @nroPagina ROWS FETCH FIRST @items ROWS ONLY";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    try
                    {
                        command.Parameters.Add("@nroPagina", SqlDbType.Int).Value = nroPagina * itemsPorPagina;
                        command.Parameters.Add("@items", SqlDbType.Int).Value = itemsPorPagina;
                        var reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Propietario propietario = new Propietario
                            {
                                Id = reader.GetInt32(0),
                                DNI = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Apellido = reader.GetString(3),
                                Email = reader.GetString(4),
                                Contraseña = reader.GetString(5),
                                Telefono = reader.GetString(6),
                            };

                            propietarios.Add(propietario);
                        }
                    }
                    catch (Exception e)
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



