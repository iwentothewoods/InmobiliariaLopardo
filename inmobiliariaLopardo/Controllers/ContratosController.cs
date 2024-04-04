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
        RepositorioInquilinos rep1 = new RepositorioInquilinos();
        RepositorioInmuebles rep2 = new RepositorioInmuebles();
        var listaIDs1 = rep1.ObtenerListaIDsInquilinos();
        var listaIDs2 = rep2.ObtenerListaIDsInmuebles();
        ViewBag.ListaIDs1 = listaIDs1;
        ViewBag.ListaIDs2 = listaIDs2;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Crear(Contrato i)
    {
        try
        {
            RepositorioContratos repo = new RepositorioContratos();
            repo.Alta(i);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    public ActionResult Editar(int id)
    {
        RepositorioInquilinos rep1 = new RepositorioInquilinos();
        RepositorioInmuebles rep2 = new RepositorioInmuebles();
        var listaIDs1 = rep1.ObtenerListaIDsInquilinos();
        var listaIDs2 = rep2.ObtenerListaIDsInmuebles();
        ViewBag.ListaIDs1 = listaIDs1;
        ViewBag.ListaIDs2 = listaIDs2;
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
}