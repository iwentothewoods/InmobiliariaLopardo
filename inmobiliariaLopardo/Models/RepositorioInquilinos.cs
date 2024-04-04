using MySql.Data.MySqlClient;
using System.Data;
namespace inmobiliariaLopardo.Models;

using System;
using Microsoft.Extensions.Configuration;

public class RepositorioInquilinos
{
    readonly string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaLunaDante;";

    public RepositorioInquilinos()
    {

    }

    public IList<Inquilino> GetInquilinos()
    {
        var inquilinos = new List<Inquilino>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"SELECT {nameof(Inquilino.Id)}, {nameof(Inquilino.Nombre)}, {nameof(Inquilino.Apellido)}, {nameof(Inquilino.Email)}, {nameof(Inquilino.Dni)}, {nameof(Inquilino.Telefono)} FROM inquilinos";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inquilinos.Add(new Inquilino
                        {
                            Id = reader.GetInt32(nameof(Inquilino.Id)),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            Email = reader.GetString(nameof(Inquilino.Email)),
                            Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                            Dni = reader.GetString(nameof(Inquilino.Dni))

                        });
                    }
                }
            }
        }
        return inquilinos;
    }

    public Inquilino? GetInquilino(int id)
    {
        Inquilino? inquilinos = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"SELECT {nameof(Inquilino.Id)}, {nameof(Inquilino.Nombre)}, {nameof(Inquilino.Apellido)}, {nameof(Inquilino.Email)}, {nameof(Inquilino.Dni)}, {nameof(Inquilino.Telefono)} FROM inquilinos WHERE {nameof(Inquilino.Id)} = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        inquilinos = new Inquilino
                        {
                            Id = reader.GetInt32(nameof(Inquilino.Id)),
                            Nombre = reader.GetString(nameof(Inquilino.Nombre)),
                            Apellido = reader.GetString(nameof(Inquilino.Apellido)),
                            Email = reader.GetString(nameof(Inquilino.Email)),
                            Telefono = reader.GetString(nameof(Inquilino.Telefono)),
                            Dni = reader.GetString(nameof(Inquilino.Dni))

                        };
                    }
                }
            }
        }
        return inquilinos;
    }

    public int Alta(Inquilino i)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"INSERT INTO inquilinos ({nameof(Inquilino.Nombre)}, {nameof(Inquilino.Apellido)}, {nameof(Inquilino.Dni)}, 
            {nameof(Inquilino.Email)}, {nameof(Inquilino.Telefono)}) VALUES (@{nameof(Inquilino.Nombre)}, @{nameof(Inquilino.Apellido)}, 
            @{nameof(Inquilino.Dni)}, @{nameof(Inquilino.Email)}, @{nameof(Inquilino.Telefono)}); SELECT LAST_INSERT_ID();";
            
            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", i.Nombre);
                command.Parameters.AddWithValue("@apellido", i.Apellido);
                command.Parameters.AddWithValue("@dni", i.Dni);
                command.Parameters.AddWithValue("@telefono", i.Telefono);
                command.Parameters.AddWithValue("@email", i.Email);

                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                i.Id = res;
                connection.Close();
            }
        }
        return res;
    }

    public bool Baja(int inquilinoId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = "DELETE FROM inquilinos WHERE Id = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", inquilinoId);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                return rowsAffected > 0;
            }
        }
    }

    public bool Modificacion(Inquilino p)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE inquilinos SET 
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

    public List<int> ObtenerListaIDsInquilinos()
    {
        List<int> listaIDs = new List<int>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"SELECT {nameof(Inquilino.Id)} FROM inquilinos";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaIDs.Add(reader.GetInt32(nameof(Inquilino.Id)));
                    }
                }
            }
        }
        return listaIDs;
    }
}