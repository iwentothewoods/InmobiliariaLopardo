using MySql.Data.MySqlClient;
using System.Data;
namespace inmobiliariaLopardo.Models;

using System;
using Microsoft.Extensions.Configuration;


public class RepositorioPropietarios
{

    readonly string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaLunaDante;";

    public RepositorioPropietarios()
    {

    }

    //Listar Propietarios
    public IList<Propietario> GetPropietarios()
    {
        var propietarios = new List<Propietario>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"SELECT {nameof(Propietario.Id)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Email)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Telefono)} FROM propietarios";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        propietarios.Add(new Propietario
                        {
                            Id = reader.GetInt32(nameof(Propietario.Id)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Email = reader.GetString(nameof(Propietario.Email)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Dni = reader.GetString(nameof(Propietario.Dni))

                        });
                    }
                }
            }
        }
        return propietarios;
    }

    public Propietario? GetPropietario(int id)
    {
        Propietario? propietario = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"SELECT {nameof(Propietario.Id)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Email)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Telefono)} FROM propietarios WHERE {nameof(Propietario.Id)} = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        propietario = new Propietario
                        {
                            Id = reader.GetInt32(nameof(Propietario.Id)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Email = reader.GetString(nameof(Propietario.Email)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Dni = reader.GetString(nameof(Propietario.Dni))

                        };
                    }
                }
            }
        }
        return propietario;
    }


    //Crear Propietario
    public int Alta(Propietario p)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"INSERT INTO propietarios ({nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Dni)}, 
            {nameof(Propietario.Email)}, {nameof(Propietario.Telefono)}) VALUES (@{nameof(Propietario.Nombre)}, @{nameof(Propietario.Apellido)}, 
            @{nameof(Propietario.Dni)}, @{nameof(Propietario.Email)}, @{nameof(Propietario.Telefono)}); SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", p.Nombre);
                command.Parameters.AddWithValue("@apellido", p.Apellido);
                command.Parameters.AddWithValue("@dni", p.Dni);
                command.Parameters.AddWithValue("@telefono", p.Telefono);
                command.Parameters.AddWithValue("@email", p.Email);

                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                p.Id = res;
                connection.Close();
            }
        }
        return res;
    }

    //Eliminar Propietario
    public bool Baja(int propietarioId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = "DELETE FROM propietarios WHERE Id = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", propietarioId);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                return rowsAffected > 0;
            }
        }
    }

    //Modificar Propietario
    public bool Modificacion(Propietario p)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE propietarios SET 
                      Nombre = @nombre,
                      Apellido = @apellido,
                      Dni = @dni,
                      Email = @email,
                      Telefono = @telefono
                      WHERE Id = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@nombre", p.Nombre);
                command.Parameters.AddWithValue("@apellido", p.Apellido);
                command.Parameters.AddWithValue("@dni", p.Dni);
                command.Parameters.AddWithValue("@telefono", p.Telefono);
                command.Parameters.AddWithValue("@email", p.Email);
                command.Parameters.AddWithValue("@id", p.Id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                return rowsAffected > 0;
            }
        }
    }

    public List<int> ObtenerListaIDsPropietarios()
    {
        List<int> listaIDs = new List<int>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"SELECT {nameof(Propietario.Id)} FROM propietarios";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaIDs.Add(reader.GetInt32(nameof(Propietario.Id)));
                    }
                }
            }
        }
        return listaIDs;
    }


}
