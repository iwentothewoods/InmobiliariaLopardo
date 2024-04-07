using System.Windows.Markup;
using Microsoft.AspNetCore.Mvc;
using inmobiliariaLopardo.Models;

namespace inmobiliariaLopardo.Controllers;

public class ContratosController : Controller
{
    private readonly ILogger<ContratosController> _logger;

    public ContratosController(ILogger<ContratosController> logger)
    {
        _logger = logger;
    }



    public IActionResult Index()
    {
        RepositorioContratos rp = new RepositorioContratos();
        var lista = rp.GetContratos();

        return View(lista);
    }


    public ActionResult Crear()
    {
        RepositorioInquilinos repInquilinos = new RepositorioInquilinos();
        var listaInquilinos = repInquilinos.GetInquilinos();

        RepositorioInmuebles repInmuebles = new RepositorioInmuebles();
        var listaInmuebles = repInmuebles.GetInmuebles();

        ViewBag.ListaInquilinos = listaInquilinos;
        ViewBag.ListaInmuebles = listaInmuebles;

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Crear(Contrato c)
    {
        try
        {
            RepositorioContratos repo = new RepositorioContratos();
            repo.Alta(c);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    public ActionResult Editar(int id)
    {

        RepositorioInquilinos rep = new RepositorioInquilinos();
        var listaInquilinos = rep.GetInquilinos();
        ViewBag.listaInquilinos = listaInquilinos;

        RepositorioPropietarios rep1 = new RepositorioPropietarios();
        var listaPropietarios = rep1.GetPropietarios();
        ViewBag.ListaPropietarios = listaPropietarios;

        RepositorioContratos repo = new RepositorioContratos();
        var contrato = repo.GetContrato(id);
        return View(contrato);
    }




    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(int id, Contrato i)
    {
        try
        {
            RepositorioContratos repo = new RepositorioContratos();
            repo.Modificacion(i);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    public ActionResult Eliminar(int id)
    {
        RepositorioContratos repo = new RepositorioContratos();
        var contrato = repo.GetContrato(id);
        if (contrato == null)
        {
            return NotFound();
        }
        RepositorioInquilinos repInquilinos = new RepositorioInquilinos();
        var inquilino = repInquilinos.GetInquilino(contrato.InquilinoId);

        RepositorioInmuebles repInmuebles = new RepositorioInmuebles();
        var inmueble = repInmuebles.GetInmueble(contrato.InmuebleId);

        ViewBag.Inquilino = inquilino;
        ViewBag.Inmueble = inmueble;
        
        return View(contrato);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Eliminar(int id, Contrato i)
    {
        try
        {
            RepositorioContratos repo = new RepositorioContratos();
            repo.Baja(id);
            TempData["Mensaje"] = "Eliminación realizada correctamente";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    [Route("contratos/detalles/{id}")]
    public ActionResult Detalles(int id)
    {
        RepositorioContratos repo = new RepositorioContratos();
        var contrato = repo.GetContrato(id);

        if (contrato != null)
        {
            RepositorioInquilinos repInquilinos = new RepositorioInquilinos();
            var inquilino = repInquilinos.GetInquilino(contrato.InquilinoId);
            contrato.Inquilino = inquilino;

            RepositorioInmuebles repInmuebles = new RepositorioInmuebles();
            var inmueble = repInmuebles.GetInmueble(contrato.InmuebleId);
            contrato.Inmueble = inmueble;

            RepositorioPropietarios repPropietarios = new RepositorioPropietarios();
            var propietario = repPropietarios.GetPropietario(contrato.Inmueble.PropietarioId);
            contrato.Inmueble.Propietario = propietario;
        }

        return View(contrato);
    }

}