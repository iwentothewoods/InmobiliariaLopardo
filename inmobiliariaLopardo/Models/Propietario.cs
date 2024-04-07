namespace inmobiliariaLopardo.Models;

public class Propietario : Persona
{
    public int Id { get; set; }
    public List<Inmueble> Inmuebles { get; set; }
    public Propietario()
    {
        Inmuebles = new List<Inmueble>();
    }
}
