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
                c.Id, c.InquilinoId, c.InmuebleId, c.FechaInicio, c.FechaFin, c.Monto,
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
                            Monto = reader.GetDecimal("Monto"),
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

    public IList<Contrato> GetContratosActivos(DateTime fin)
    {
        List<Contrato> contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"
            SELECT
                 c.*,
                 i.Nombre AS NombreInquilino, i.Apellido AS ApellidoInquilino,
                 im.Direccion AS DireccionInmueble,
                 p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario
            FROM Contratos c
            INNER JOIN inquilinos i ON c.InquilinoId = i.Id
            INNER JOIN inmuebles im ON c.InmuebleId = im.Id
            INNER JOIN propietarios p ON im.PropietarioId = p.Id
            WHERE c.FechaFin > @Fin";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Fin", fin);

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
                            Monto = reader.GetDecimal("Monto"),
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

    public IList<Contrato> GetContratosInactivos(DateTime fin)
    {
        List<Contrato> contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"
            SELECT
                 c.*,
                 i.Nombre AS NombreInquilino, i.Apellido AS ApellidoInquilino,
                 im.Direccion AS DireccionInmueble,
                 p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario
            FROM Contratos c
            INNER JOIN inquilinos i ON c.InquilinoId = i.Id
            INNER JOIN inmuebles im ON c.InmuebleId = im.Id
            INNER JOIN propietarios p ON im.PropietarioId = p.Id
            WHERE c.FechaFin <= @Fin";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Fin", fin);

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
                            Monto = reader.GetDecimal("Monto"),
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

    public IList<Contrato> GetContratosAVencer(DateTime vencimiento)
    {
        List<Contrato> contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"
            SELECT
                 c.*,
                 i.Nombre AS NombreInquilino, i.Apellido AS ApellidoInquilino,
                 im.Direccion AS DireccionInmueble,
                 p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario
            FROM Contratos c
            INNER JOIN inquilinos i ON c.InquilinoId = i.Id
            INNER JOIN inmuebles im ON c.InmuebleId = im.Id
            INNER JOIN propietarios p ON im.PropietarioId = p.Id
            WHERE c.FechaFin <= @Vencimiento";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Vencimiento", vencimiento);

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
                            Monto = reader.GetDecimal("Monto"),
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

    public IList<Contrato> GetContratosPorRango(DateTime ini, DateTime fin)
    {
        List<Contrato> contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"
            SELECT
                 c.*,
                 i.Nombre AS NombreInquilino, i.Apellido AS ApellidoInquilino,
                 im.Direccion AS DireccionInmueble,
                 p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario
            FROM Contratos c
            INNER JOIN inquilinos i ON c.InquilinoId = i.Id
            INNER JOIN inmuebles im ON c.InmuebleId = im.Id
            INNER JOIN propietarios p ON im.PropietarioId = p.Id
            WHERE c.FechaInicio <= @fin AND c.FechaFin >= @ini";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Ini", ini);
                command.Parameters.AddWithValue("@Fin", fin);

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
                            Monto = reader.GetDecimal("Monto"),
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

    public IList<Contrato> GetPorInmuebles(int inmId)
    {
        List<Contrato> contratos = new List<Contrato>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"
            SELECT
                 c.*,
                 i.Nombre AS NombreInquilino, i.Apellido AS ApellidoInquilino,
                 im.Direccion AS DireccionInmueble,
                 p.Nombre AS NombrePropietario, p.Apellido AS ApellidoPropietario
            FROM Contratos c
            INNER JOIN inquilinos i ON c.InquilinoId = i.Id
            INNER JOIN inmuebles im ON c.InmuebleId = im.Id
            INNER JOIN propietarios p ON im.PropietarioId = p.Id
            WHERE c.InmuebleId = @InmId";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@InmId", inmId);

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
                            Monto = reader.GetDecimal("Monto"),
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
            var sql = $"SELECT {nameof(Contrato.Id)}, {nameof(Contrato.InquilinoId)}, {nameof(Contrato.InmuebleId)}, {nameof(Contrato.FechaInicio)}, {nameof(Contrato.FechaFin)}, {nameof(Contrato.Monto)} FROM contratos WHERE {nameof(Contrato.Id)} = @id";
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
                            Monto = reader.GetDecimal(nameof(Contrato.Monto)),
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
            var sql = @"INSERT INTO Contratos (InquilinoId, InmuebleId, FechaInicio, FechaFin, Monto)
                    VALUES (@InquilinoId, @InmuebleId, @FechaInicio, @FechaFin, @Monto);
                    SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@InquilinoId", contrato.InquilinoId);
                command.Parameters.AddWithValue("@InmuebleId", contrato.InmuebleId);
                command.Parameters.AddWithValue("@FechaInicio", contrato.FechaInicio);
                command.Parameters.AddWithValue("@FechaFin", contrato.FechaFin);
                command.Parameters.AddWithValue("@Monto", contrato.Monto);

                connection.Open();
                contratoId = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return contratoId;
    }



    public bool Baja(Contrato b)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE Contratos SET
                      FechaFin = @FechaFin
                      WHERE Id = @Id";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@FechaFin", b.FechaFin);
                command.Parameters.AddWithValue("@Id", b.Id);

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

    public Pago? GetPago()
    {
        Pago? pago = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"SELECT {nameof(Pago.Id)}, {nameof(Pago.ContratoId)}, {nameof(Pago.FechaPago)}, {nameof(Pago.Importe)} FROM pagos";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pago = new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                            FechaPago = reader.GetDateTime(nameof(Pago.FechaPago)),
                            Importe = reader.GetDecimal(nameof(Pago.Importe))

                        };
                    }
                }
            }
        }
        return pago;
    }

    public IList<Pago> GetPagos()
    {
        var pagos = new List<Pago>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $"SELECT {nameof(Pago.Id)}, {nameof(Pago.ContratoId)}, {nameof(Pago.FechaPago)}, {nameof(Pago.Importe)} FROM pagos";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pagos.Add(new Pago
                        {
                            Id = reader.GetInt32(nameof(Pago.Id)),
                            ContratoId = reader.GetInt32(nameof(Pago.ContratoId)),
                            FechaPago = reader.GetDateTime(nameof(Pago.FechaPago)),
                            Importe = reader.GetDecimal(nameof(Pago.Importe))

                        });
                    }
                }
            }
        }
        return pagos;
    }

    public int Pagar(Pago pago)
    {
        int pagoId = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"INSERT INTO Pagos (ContratoId, FechaPago, Importe)
                    VALUES (@ContratoId, @FechaPago, @Importe);
                    SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@ContratoId", pago.ContratoId);
                command.Parameters.AddWithValue("@FechaPago", pago.FechaPago);
                command.Parameters.AddWithValue("@Importe", pago.Importe);

                connection.Open();
                pagoId = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return pagoId;
    }
}