using System.Windows.Markup;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;

namespace inmobiliariaLopardo.Controllers;


public class PropietariosController : Controller
{

    private readonly ILogger<PropietariosController> _logger;

    public PropietariosController(ILogger<PropietariosController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        RepositorioPropietarios rp = new RepositorioPropietarios();
        var lista = rp.GetPropietarios();
        return View(lista);
    }

}



