namespace inmobiliariaLopardo.Models;

public class Propietario : Persona
{
    public int Id { get; set; }
    public List<Inmueble> Inmuebles { get; set; }
    public Propietario()
    {
        Inmuebles = new List<Inmueble>();
    }

    //Agregué un ToString para no tener que editar los nombres de los propietarios cada vez.
    public override string ToString()
    {
        var res = $"{Nombre}" + "{Apellido}";
        return res;
    }
}
