using MySql.Data.MySqlClient;
using System.Data;
namespace inmobiliariaLopardo.Models;

using System;
using Microsoft.Extensions.Configuration;

public class RepositorioContratos
{
    readonly string connectionString = "Server=localhost;User=root;Password=;Database=inmobiliariaLunaDante;";

    public RepositorioContratos()
    {

    }

    public IList<Contrato> GetContratos()
    {
        var contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"
            SELECT 
                c.Id, c.InquilinoId, c.InmuebleId, c.FechaInicio, c.FechaFin,
                i.Nombre AS NombreInquilino, i.Apellido AS ApellidoInquilino,
                im.Direccion AS DireccionInmueble,
                p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario
            FROM contratos c
            INNER JOIN inquilinos i ON c.InquilinoId = i.Id
            INNER JOIN inmuebles im ON c.InmuebleId = im.Id
            INNER JOIN propietarios p ON im.PropietarioId = p.Id";

            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contratos.Add(new Contrato
                        {
                            Id = reader.GetInt32("Id"),
                            InquilinoId = reader.GetInt32("InquilinoId"),
                            InmuebleId = reader.GetInt32("InmuebleId"),
                            FechaInicio = reader.GetDateTime("FechaInicio"),
                            FechaFin = reader.GetDateTime("FechaFin"),
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("NombreInquilino"),
                                Apellido = reader.GetString("ApellidoInquilino")
                            },
                            Inmueble = new Inmueble
                            {
                                Direccion = reader.GetString("DireccionInmueble"),
                                Propietario = new Propietario
                                {
                                    Nombre = reader.GetString("NombrePropietario"),
                                    Apellido = reader.GetString("ApellidoPropietario")
                                }
                            }
                        });
                    }
                }
            }
        }
        return contratos;
    }



    public Contrato? GetContrato(int id)
    {
        Contrato? Contrato = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"SELECT {nameof(Contrato.Id)}, {nameof(Contrato.InquilinoId)}, {nameof(Contrato.InmuebleId)}, {nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaFin)} FROM contratos WHERE {nameof(Contrato.Id)} = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Contrato = new Contrato
                        {
                            Id = reader.GetInt32(nameof(Contrato.Id)),
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                            InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                            FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                            FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                        };
                    }
                }
            }
        }
        return Contrato;
    }

    public int Alta(Contrato contrato)
    {
        int contratoId = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"INSERT INTO Contratos (InquilinoId, InmuebleId, FechaInicio, FechaFin)
                    VALUES (@InquilinoId, @InmuebleId, @FechaInicio, @FechaFin);
                    SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
                command.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
                command.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
                command.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);

                connection.Open();
                contratoId = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return contratoId;
    }



    public bool Baja(int ContratoId)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = "DELETE FROM Contratos WHERE Id = @id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", ContratoId);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                return rowsAffected > 0;
            }
        }
    }

    public bool Modificacion(Contrato i)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE Contratos SET 
                      FechaInicio = @FechaInicio,
                      FechaFin = @FechaFin
                      WHERE Id = @Id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@FechaInicio", i.FechaInicio);
                command.Parameters.AddWithValue("@FechaFin", i.FechaFin);
                command.Parameters.AddWithValue("@Id", i.Id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                return rowsAffected > 0;
            }
        }
    }

}