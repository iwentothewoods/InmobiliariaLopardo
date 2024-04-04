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

            var sql = $"SELECT {nameof(Contrato.Id)}, {nameof(Contrato.InquilinoId)}, {nameof(Contrato.InmuebleId)}, {nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaFin)}, {nameof(Contrato.FechaTerminacion)} FROM contratos";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        contratos.Add(new Contrato
                        {
                            Id = reader.GetInt32(nameof(Contrato.Id)),
                            InquilinoId = reader.GetInt32(nameof(Contrato.InquilinoId)),
                            InmuebleId = reader.GetInt32(nameof(Contrato.InmuebleId)),
                            FechaInicio = reader.GetDateTime(nameof(Contrato.FechaInicio)),
                            FechaFin = reader.GetDateTime(nameof(Contrato.FechaFin)),
                            FechaTerminacion = reader.GetDateTime(nameof(Contrato.FechaTerminacion)),
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
            var sql = $"SELECT {nameof(Contrato.Id)}, {nameof(Contrato.InquilinoId)}, {nameof(Contrato.InmuebleId)}, {nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaFin)}, {nameof(Contrato.FechaTerminacion)} FROM contratos WHERE {nameof(Contrato.Id)} = @id";
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
                            FechaTerminacion = reader.GetDateTime(nameof(Contrato.FechaTerminacion)),
                        };
                    }
                }
            }
        }
        return Contrato;
    }

    public int Alta(Contrato i)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"INSERT INTO Contratos (InquilinoId, InmuebleId, FechaInicio, FechaFin, FechaTerminacion)
                    VALUES (@InquilinoId, @InmuebleId, @FechaInicio, @FechaFin, @FechaTerminacion);
                    SELECT LAST_INSERT_ID();";
            
            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@InquilinoId", i.InquilinoId);
                command.Parameters.AddWithValue("@InmuebleId", i.InmuebleId);
                command.Parameters.AddWithValue("@FechaInicio", i.FechaInicio);
                command.Parameters.AddWithValue("@FechaFin", i.FechaFin);
                command.Parameters.AddWithValue("@FechaTerminacion", i.FechaTerminacion);

                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                i.Id = res;
                connection.Close();
            }
        }
        return res;
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
                      InquilinoId = @InquilinoId,
                      InmuebleId = @InmuebleId,
                      FechaInicio = @FechaInicio,
                      FechaFin = @FechaFin,
                      FechaTerminacion = @FechaTerminacion
                      WHERE Id = @Id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@InquilinoId", i.InquilinoId);
                command.Parameters.AddWithValue("@InmuebleId", i.InmuebleId);
                command.Parameters.AddWithValue("@FechaInicio", i.FechaInicio);
                command.Parameters.AddWithValue("@FechaFin", i.FechaFin);
                command.Parameters.AddWithValue("@FechaTerminacion", i.FechaTerminacion);
                command.Parameters.AddWithValue("@Id", i.Id);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                return rowsAffected > 0;
            }
        }
    }

}