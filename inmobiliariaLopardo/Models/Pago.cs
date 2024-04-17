using System.ComponentModel.DataAnnotations;

namespace inmobiliariaLopardo.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }

        public Contrato Contrato { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; }

        public decimal Importe { get; set; }

        public Pago()
        {

        }

        public Pago(int contratoId, DateTime fechaPago, decimal importe)
        {
            ContratoId = contratoId;
            FechaPago = fechaPago;
            Importe = importe;
        }
    }
}