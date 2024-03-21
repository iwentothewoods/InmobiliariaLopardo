using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace web.Models;

public class RepositorioPersona
{
        readonly String ConnectionString = "Server=localhost;Database=inmobiliariaLopardo;User=root;Password=;";

    public RepositorioPersona(){

    }

    public IList<Persona> GetPersonas()
    {
        var personas = new List<Persona>();
        using (var connection = new MySqlConnection(ConnectionString))
        {
            var sql = "SELECT {nameof(Persona.Id)}, {nameof(Persona.Nombre)}, {nameof(Persona.Documento}, {nameof(Persona.Email)} FROM personas";
            using(var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using(var reader = command.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        personas.Add(new Persona
                        {
                            Id = reader.GetInt32(nameof(Persona.Id)),
                            Documento = reader.GetString(nameof(Persona.Documento)),
                            Nombre = reader.GetString(nameof(Persona.Nombre)),
                            Email = reader.GetString(nameof(Persona.Email))
                        });
                    }
                }
            }
        }
        return personas;
    }


}