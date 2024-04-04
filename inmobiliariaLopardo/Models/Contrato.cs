using System.ComponentModel.DataAnnotations;

namespace inmobiliariaLopardo.Models;

public class Contrato
{
    public int Id {get; set;}
    public int InquilinoId {get; set;}

    public int InmuebleId {get; set;}

    [DataType(DataType.Date)]
    public DateTime FechaInicio {get; set;}

    [DataType(DataType.Date)]
    public DateTime FechaFin {get; set;}

    [DataType(DataType.Date)]
    public DateTime FechaTerminacion {get; set;}

    public Contrato()
    {
        
    }

    public Contrato(int inquilinoId, int inmuebleId, DateTime fechaInicio, DateTime fechaFin, DateTime fechaTerminacion)
    {
        InquilinoId = inquilinoId;
        InmuebleId = inmuebleId;
        FechaInicio = fechaInicio;
        FechaFin = fechaFin;
        FechaTerminacion = fechaTerminacion;
    }

}