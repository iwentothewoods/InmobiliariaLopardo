using MySql.Data.MySqlClient;

namespace Proyecto.Models
{
	public class RepositorioPersona
	{
		protected readonly string connectionString;
		public RepositorioPersona()
		{
			connectionString = "Server=localhost;User=root;Password=;Database=Inmobiliaria;SslMode=None";
		}

	}
}