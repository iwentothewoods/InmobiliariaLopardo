using System;
namespace inmobiliariaLopardo.Models;
public class Inmueble
{
    public int Id { get; set; }
    public int PropietarioId { get; set; }
    public string Direccion { get; set; }
    public UsoInmueble Uso { get; set; }
    public TipoInmueble Tipo { get; set; }
    public int? Ambientes { get; set; }
    public double? Latitud { get; set; }
    public double? Longitud { get; set; }
    public decimal Precio { get; set; }
    public bool Activo { get; set; }
    public bool Disponible { get; set; }

    public Propietario? Propietario { get; set; } //Agregué un propietario entero para poder traer nombre y apellido en las tablas / ABM
    public Inmueble()
    {

    }

    public Inmueble(int id, int propietarioId, Propietario propietario, string direccion, UsoInmueble uso, TipoInmueble tipo, int? ambientes, double? latitud, double? longitud, decimal precio, bool activo, bool disponible)
    {
        Id = id;
        PropietarioId = propietarioId;
        Propietario = propietario;
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
