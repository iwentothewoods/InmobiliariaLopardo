using System.ComponentModel.DataAnnotations;
using System;

namespace inmobiliariaLopardo.Models
{
    public class Contrato
    {
        public int Id { get; set; }
        public int InquilinoId { get; set; }
        public Inquilino Inquilino { get; set; }  // Agrego inquilino entero para las tablas y ABM

        public int InmuebleId { get; set; }
        public Inmueble Inmueble { get; set; }  // Agrego inmueble entero para las tablas y ABM > de acá sacamos propietario luego

        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaTerminacion { get; set; }

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
}
