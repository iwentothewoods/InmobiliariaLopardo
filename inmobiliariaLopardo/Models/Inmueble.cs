using System;
namespace inmobiliariaLopardo.Models;
public class Inmueble
{
    public int Id { get; set; }
    public int PropietarioId { get; set; }
    public string Direccion { get; set; }
    public UsoInmueble Uso { get; set; }  // Enumerador para Uso
    public TipoInmueble Tipo { get; set; }  // Enumerador para Tipo
    public int? Ambientes { get; set; }
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public double Precio { get; set; }
    public bool Activo { get; set; }
    public bool Disponible { get; set; }

    public Inmueble()
    {
        
    }

    public Inmueble(int propietarioId, string direccion, UsoInmueble uso, TipoInmueble tipo, int? ambientes, double? latitud, double? longitud, double precio, bool activo, bool disponible)
    {
        PropietarioId = propietarioId;
        Direccion = direccion;
        Uso = uso;
        Tipo = tipo;
        Ambientes = ambientes;
        Latitud = latitud;
        Longitud = longitud;
        Precio = precio;
        Activo = activo;
        Disponible = disponible;
    }
}

// Enumerador para Uso de Inmueble
public enum UsoInmueble
{
    Comercial = 1,
    Personal = 2
}

// Enumerador para Tipo de Inmueble
public enum TipoInmueble
{
    Casa = 1,
    Oficina = 2,
    Departamento = 3,
    Almacen = 4
}
