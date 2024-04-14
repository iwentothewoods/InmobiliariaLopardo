using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace inmobiliariaLopardo.Models;
public enum Roles
{
    Administrador = 1,
    Empleado = 2,
}

public class Usuario
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string? Clave { get; set; }
    public string? Avatar { get; set; }
    
    public IFormFile? AvatarFile { get; set; }
     public int Rol { get; set; }

     //Si es mayor a cero, devuelve el toString del enum, si no, lo devuelve vacío
    public string RolNombre => Rol > 0 ? ((Roles)Rol).ToString() : "";

    //Devuelve el int (clave numérica del enum), junto con el string (valor del mismo)
    public static IDictionary<int, string> ObtenerRoles()
    {
        SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
        Type tipoEnumRol = typeof(Roles);
        foreach (var valor in Enum.GetValues(tipoEnumRol))
        {
            roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
        }
        return roles;
    }
}
