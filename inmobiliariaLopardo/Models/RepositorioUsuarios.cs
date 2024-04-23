using MySql.Data.MySqlClient;
using System.Data;
namespace inmobiliariaLopardo.Models;

using System;
using Microsoft.Extensions.Configuration;


public class RepositorioUsuarios
{


    readonly string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaLunaDante;";

    private string[] enumRol = Enum.GetNames(typeof(Roles));
    public List<string> getEnumRol()
    {
        return enumRol.ToList();
    }

    public RepositorioUsuarios()
    {

    }

    //Listar Usuarios
    public IList<Usuario> GetUsuarios()
    {
        var Usuarios = new List<Usuario>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"SELECT {nameof(Usuario.Id)}, {nameof(Usuario.Nombre)}, {nameof(Usuario.Apellido)}, {nameof(Usuario.Email)}, {nameof(Usuario.Clave)}, {nameof(Usuario.Rol)}, {nameof(Usuario.Avatar)} FROM Usuarios";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Usuarios.Add(new Usuario
                        {
                            Id = reader.GetInt32(nameof(Usuario.Id)),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Email = reader.GetString(nameof(Usuario.Email)),
                            Clave = reader.GetString(nameof(Usuario.Clave)),
                            Rol = reader.GetInt32(nameof(Usuario.Rol)),
                            Avatar = reader.GetString(nameof(Usuario.Avatar))
                        });
                    }
                }
            }
        }
        return Usuarios;
    }

    public Usuario GetUsuario(int id)
    {
        Usuario? usuario = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"SELECT {nameof(Usuario.Id)}, {nameof(Usuario.Nombre)}, {nameof(Usuario.Apellido)}, {nameof(Usuario.Email)}, {nameof(Usuario.Clave)}, {nameof(Usuario.Rol)}, {nameof(Usuario.Avatar)} FROM usuarios WHERE {nameof(Usuario.Id)} = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            Id = reader.GetInt32(nameof(Usuario.Id)),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Email = reader.GetString(nameof(Usuario.Email)),
                            Clave = reader.GetString(nameof(Usuario.Clave)),
                            Rol = reader.GetInt32(nameof(Usuario.Rol)),
                            Avatar = reader.GetString(nameof(Usuario.Avatar))
                        };
                    }
                }
            }
        }
        return usuario;
    }

    public int Alta(Usuario u)
    {
        var res = -1;
        bool flag = CorreoRepetido(u);

        if (!flag)
        {
            res = -2;
            return res;
        }

        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"INSERT INTO usuarios ({nameof(Usuario.Nombre)}, {nameof(Usuario.Apellido)}, {nameof(Usuario.Email)}, 
        {nameof(Usuario.Clave)}, {nameof(Usuario.Rol)}, {nameof(Usuario.Avatar)}) VALUES (@{nameof(Usuario.Nombre)}, @{nameof(Usuario.Apellido)}, 
        @{nameof(Usuario.Email)}, @{nameof(Usuario.Clave)}, @{nameof(Usuario.Rol)}, @{nameof(Usuario.Avatar)}); SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@nombre", u.Nombre);
                command.Parameters.AddWithValue("@apellido", u.Apellido);
                command.Parameters.AddWithValue("@email", u.Email);
                command.Parameters.AddWithValue("@clave", u.Clave);
                command.Parameters.AddWithValue("@rol", u.Rol);
                if (String.IsNullOrEmpty(u.Avatar))
                {
                    command.Parameters.AddWithValue("@avatar", "");
                }
                else
                {
                    command.Parameters.AddWithValue("@avatar", u.Avatar);
                }

                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                u.Id = res;
            }
        }
        return res;
    }

    public bool Baja(int usuarioId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = "DELETE FROM usuarios WHERE Id = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", usuarioId);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
    }

    public bool Modificacion(Usuario u)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE usuarios SET 
                  Nombre = @nombre,
                  Apellido = @apellido,
                  Email = @email,
                  Rol = @rol
                  WHERE Id = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@nombre", u.Nombre);
                command.Parameters.AddWithValue("@apellido", u.Apellido);
                command.Parameters.AddWithValue("@email", u.Email);
                command.Parameters.AddWithValue("@rol", u.Rol);
                command.Parameters.AddWithValue("@id", u.Id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
    }

    public Usuario ObtenerPorEmail(string email)
    {
        Usuario? usuario = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"SELECT {nameof(Usuario.Id)}, {nameof(Usuario.Nombre)}, {nameof(Usuario.Apellido)}, {nameof(Usuario.Email)}, {nameof(Usuario.Clave)}, {nameof(Usuario.Rol)}, {nameof(Usuario.Avatar)} FROM usuarios WHERE {nameof(Usuario.Email)} = @email";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@email", email);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            Id = reader.GetInt32(nameof(Usuario.Id)),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Email = reader.GetString(nameof(Usuario.Email)),
                            Clave = reader.GetString(nameof(Usuario.Clave)),
                            Rol = reader.GetInt32(nameof(Usuario.Rol)),
                            Avatar = reader.GetString(nameof(Usuario.Avatar))
                        };
                    }
                }
            }
        }
        return usuario;
    }

    public bool CorreoRepetido(Usuario usuario)
    {
        bool correoRepetido = false;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            string query = @"SELECT Id FROM Usuarios WHERE Email = @Email;";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", usuario.Email);

                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Si el correo ya existe en la base de datos, correoRepetido será true
                            correoRepetido = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al comprobar correo repetido: " + ex.Message);
                }
            }
        }

        return correoRepetido;
    }


    public void CambiarClave(Usuario usuario, string nuevaClave)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"UPDATE Usuarios SET Clave = @NuevaClave WHERE Id = @Id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@NuevaClave", nuevaClave);
                command.Parameters.AddWithValue("@Id", usuario.Id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }

    public void CambiarAvatar(Usuario usuario, string nuevoAvatar)
    {
        using (var connection = new MySqlConnection(connectionString))
        {

            var sql = $"UPDATE Usuarios SET Avatar = @NuevoAvatar WHERE Id = @Id";


            using (var command = new MySqlCommand(sql, connection))
            {

                command.Parameters.AddWithValue("@NuevoAvatar", nuevoAvatar);
                command.Parameters.AddWithValue("@Id", usuario.Id);


                connection.Open();

                command.ExecuteNonQuery();
            }
        }
    }



}