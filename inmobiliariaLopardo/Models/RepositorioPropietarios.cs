using MySql.Data.MySqlClient;
using System.Data;
namespace Proyecto.Models;
using Microsoft.Extensions.Configuration;


public class RepositorioPropietarios
{
    readonly string connectionString = "Server=localhost;User=root;Password=;Database=InmobiliariaLopardo;";

    public RepositorioPropietarios()
    {
        
    }

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

}
