using System;
using inmobiliariaLopardo.Models;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace inmobiliariaLopardo.ViewModels;
public class ViewModelPagoContrato
 {
    public Contrato contrato{ get; set; }

    public Pago pago{ get; set; }

    public IEnumerable<Pago> lpago { get; set; }

 }

 

 