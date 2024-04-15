using MySql.Data.MySqlClient;
using System.Data;
namespace inmobiliariaLopardo.Models;

using System;
using Microsoft.Extensions.Configuration;


public class RepositorioInmuebles
{
    readonly string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaLunaDante;";

    public RepositorioInmuebles()
    {

    }

    //Listar Inmuebles
    public IList<Inmueble> GetInmuebles()
    {
        var inmuebles = new List<Inmueble>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"SELECT i.Id AS InmuebleId, i.PropietarioId, i.Direccion, i.Uso, i.Tipo, i.Latitud, i.Longitud, i.Ambientes, i.Precio, i.Activo, 
                    i.Disponible, p.Nombre AS PropietarioNombre, p.Apellido AS PropietarioApellido
                    FROM Inmuebles i
                    INNER JOIN Propietarios p ON i.PropietarioId = p.Id";

            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        
                        inmuebles.Add(new Inmueble
                        {
                            Id = reader.GetInt32("InmuebleId"),
                            PropietarioId = reader.GetInt32("PropietarioId"),
                            Direccion = reader.GetString("Direccion"),
                            Uso = (UsoInmueble)reader.GetInt32("Uso"),
                            Tipo = (TipoInmueble)reader.GetInt32("Tipo"),
                            Latitud = reader.GetDouble("Latitud"),
                            Longitud = reader.GetDouble("Longitud"),
                            Ambientes = reader.GetInt32("Ambientes"),
                            Precio = reader.GetDecimal("Precio"),
                            Disponible = reader.GetBoolean("Disponible"),
                            Activo = reader.GetBoolean("Activo"),
                            Propietario = new Propietario
                            {
                                Id = reader.GetInt32("PropietarioId"),
                                Nombre = reader.GetString("PropietarioNombre"),
                                Apellido = reader.GetString("PropietarioApellido")
                            }
                        });
                    }
                }
            }
        }
        return inmuebles;
    }


    public Inmueble? GetInmueble(int id)
    {
        Inmueble? Inmueble = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"SELECT {nameof(Inmueble.Id)}, {nameof(Inmueble.PropietarioId)}, {nameof(Inmueble.Direccion)},{nameof(Inmueble.Uso)}, {nameof(Inmueble.Tipo)}, {nameof(Inmueble.Latitud)}, {nameof(Inmueble.Longitud)}, {nameof(Inmueble.Ambientes)}, {nameof(Inmueble.Precio)}, {nameof(Inmueble.Activo)}, {nameof(Inmueble.Disponible)} FROM inmuebles WHERE {nameof(Inmueble.Id)} = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(nameof(Inmueble.Id)),
                            PropietarioId = reader.GetInt32(nameof(Inmueble.PropietarioId)),
                            Direccion = reader.GetString(nameof(Inmueble.Direccion)),
                            Uso = (UsoInmueble)reader.GetInt32(nameof(Inmueble.Uso)),
                            Tipo = (TipoInmueble)reader.GetInt32(nameof(Inmueble.Tipo)),
                            Latitud = reader.GetDouble(nameof(Inmueble.Latitud)),
                            Longitud = reader.GetDouble(nameof(Inmueble.Longitud)),
                            Ambientes = reader.GetInt32(nameof(Inmueble.Ambientes)),
                            Precio = reader.GetDecimal(nameof(Inmueble.Precio)),
                            Disponible = reader.GetBoolean(nameof(Inmueble.Disponible)),
                            Activo = reader.GetBoolean(nameof(Inmueble.Activo))
                        };
                    }
                }
            }
        }
        return Inmueble;
    }

    public List<Inmueble> GetInmueblesPorPropietario(int propietarioId)
    {
        var inmuebles = new List<Inmueble>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"SELECT Id, Direccion, Uso, Tipo, Precio, Activo, Disponible 
            FROM Inmuebles WHERE PropietarioId = @propietarioId";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@propietarioId", propietarioId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inmuebles.Add(new Inmueble
                        {
                            Id = reader.GetInt32("Id"),
                            Direccion = reader.GetString("Direccion"),
                            Uso = (UsoInmueble)reader.GetInt32("Uso"),
                            Tipo = (TipoInmueble)reader.GetInt32("Tipo"),
                            Precio = reader.GetDecimal("Precio"),
                            Disponible = reader.GetBoolean("Disponible"),
                            Activo = reader.GetBoolean("Activo")
                        });
                    }
                }
            }
        }
        return inmuebles;
    }

    public List<Inmueble> GetInmueblesPorTipo(int tipoId)
    {
        List<Inmueble> listaInmuebles = new List<Inmueble>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"
            SELECT 
                i.*, 
                p.Nombre AS NombrePropietario, 
                p.Apellido AS ApellidoPropietario 
            FROM Inmuebles i
            INNER JOIN Propietarios p ON i.PropietarioId = p.Id
            WHERE i.Tipo = @TipoId";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@TipoId", tipoId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaInmuebles.Add(new Inmueble
                        {
                            Id = reader.GetInt32("Id"),
                            Direccion = reader.GetString("Direccion"),
                            Uso = (UsoInmueble)reader.GetInt32("Uso"),
                            Tipo = (TipoInmueble)reader.GetInt32("Tipo"),
                            Precio = reader.GetDecimal("Precio"),
                            Disponible = reader.GetBoolean("Disponible"),
                            Activo = reader.GetBoolean("Activo"),
                            Propietario = new Propietario
                        {
                            Nombre = reader.GetString("NombrePropietario"),
                            Apellido = reader.GetString("ApellidoPropietario")
                        }
                        });
                    }
                }
            }
        }
        return listaInmuebles;
    }

    public List<Inmueble> GetInmueblesPorDisponibilidad(int dispId)
    {
        List<Inmueble> listaInmuebles = new List<Inmueble>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"
            SELECT 
                i.*, 
                p.Nombre AS NombrePropietario, 
                p.Apellido AS ApellidoPropietario 
            FROM Inmuebles i
            INNER JOIN Propietarios p ON i.PropietarioId = p.Id
            WHERE i.Disponible = @DispId";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@DispId", dispId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaInmuebles.Add(new Inmueble
                        {
                            Id = reader.GetInt32("Id"),
                            Direccion = reader.GetString("Direccion"),
                            Uso = (UsoInmueble)reader.GetInt32("Uso"),
                            Tipo = (TipoInmueble)reader.GetInt32("Tipo"),
                            Precio = reader.GetDecimal("Precio"),
                            Disponible = reader.GetBoolean("Disponible"),
                            Activo = reader.GetBoolean("Activo"),
                            Propietario = new Propietario
                        {
                            Nombre = reader.GetString("NombrePropietario"),
                            Apellido = reader.GetString("ApellidoPropietario")
                        }
                        });
                    }
                }
            }
        }
        return listaInmuebles;
    }



    public int Alta(Inmueble i)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"INSERT INTO Inmuebles (PropietarioId, Direccion, Uso, Tipo, Latitud, Longitud, Ambientes, Precio, Activo, Disponible)
                    VALUES (@PropietarioId, @Direccion, @Uso, @Tipo, @Latitud, @Longitud, @Ambientes, @Precio, @Activo, @Disponible);
                    SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@PropietarioId", i.PropietarioId);
                command.Parameters.AddWithValue("@Direccion", i.Direccion);
                command.Parameters.AddWithValue("@Uso", (int)i.Uso);
                command.Parameters.AddWithValue("@Tipo", (int)i.Tipo);
                command.Parameters.AddWithValue("@Latitud", i.Latitud);
                command.Parameters.AddWithValue("@Longitud", i.Longitud);
                command.Parameters.AddWithValue("@Ambientes", i.Ambientes);
                command.Parameters.AddWithValue("@Precio", i.Precio);
                command.Parameters.AddWithValue("@Activo", i.Activo ? 1 : 0); // Convierte bool a 1 o 0
                command.Parameters.AddWithValue("@Disponible", i.Disponible ? 1 : 0);

                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                i.Id = res;
                connection.Close();
            }
        }
        return res;
    }

    //Eliminar Inmueble
    public bool Baja(int InmuebleId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = "DELETE FROM Inmuebles WHERE Id = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", InmuebleId);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                return rowsAffected > 0;
            }
        }
    }

    //Modificar Inmueble

    public bool Modificacion(Inmueble i)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE Inmuebles SET 
                      PropietarioId = @PropietarioId,
                      Direccion = @Direccion,
                      Uso = @Uso,
                      Tipo = @Tipo,
                      Latitud = @Latitud,
                      Longitud = @Longitud,
                      Ambientes = @Ambientes,
                      Precio = @Precio,
                      Activo = @Activo,
                      Disponible = @Disponible
                      WHERE Id = @Id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@PropietarioId", i.PropietarioId);
                command.Parameters.AddWithValue("@Direccion", i.Direccion);
                command.Parameters.AddWithValue("@Uso", (int)i.Uso);
                command.Parameters.AddWithValue("@Tipo", (int)i.Tipo);
                command.Parameters.AddWithValue("@Latitud", i.Latitud);
                command.Parameters.AddWithValue("@Longitud", i.Longitud);
                command.Parameters.AddWithValue("@Ambientes", i.Ambientes);
                command.Parameters.AddWithValue("@Precio", i.Precio);
                command.Parameters.AddWithValue("@Activo", i.Activo ? 1 : 0);
                command.Parameters.AddWithValue("@Disponible", i.Disponible ? 1 : 0);
                command.Parameters.AddWithValue("@Id", i.Id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                return rowsAffected > 0;
            }
        }
    }

    public List<int> ObtenerListaIDsInmuebles()
    {
        List<int> listaIDs = new List<int>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"SELECT {nameof(Inmueble.Id)} FROM inmuebles";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaIDs.Add(reader.GetInt32(nameof(Inmueble.Id)));
                    }
                }
            }
        }
        return listaIDs;
    }

    public bool altaContrato(Contrato c){
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql2 = @"UPDATE Inmuebles SET
                      Disponible = @Disponible
                      WHERE Id = @Id";

            using (var command = new MySqlCommand(sql2, connection))
            {
                command.Parameters.AddWithValue("@Disponible", false);
                command.Parameters.AddWithValue("@Id", c.InmuebleId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();
                return rowsAffected > 0;
            }
        }
    }

    public bool bajaContrato(Contrato c){
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql2 = @"UPDATE Inmuebles SET
                      Disponible = @Disponible
                      WHERE Id = @Id";

            using (var command = new MySqlCommand(sql2, connection))
            {
                command.Parameters.AddWithValue("@Disponible", true);
                command.Parameters.AddWithValue("@Id", c.InmuebleId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();
                return rowsAffected > 0;
            }
        }
    }
}
