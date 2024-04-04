using System.Windows.Markup;
using Microsoft.AspNetCore.Mvc;
using inmobiliariaLopardo.Models;

namespace inmobiliariaLopardo.Controllers;

public class InmueblesController : Controller
{
    private readonly ILogger<InmueblesController> _logger;

    public InmueblesController(ILogger<InmueblesController> logger)
    {
        _logger = logger;
    }



    public IActionResult Index(string tipoPropiedad)
    {
        RepositorioInmuebles rp = new RepositorioInmuebles();
        var lista = rp.GetInmuebles();

        // Filtrar por tipo de propiedad si se ha seleccionado un valor en el dropdown
        if (!string.IsNullOrEmpty(tipoPropiedad))
        {
            lista = lista.Where(i => i.Tipo.ToString() == tipoPropiedad).ToList();
        }

        return View(lista);
    }


    // GET: Inmuebles/Crear
    public ActionResult Crear()
    {
        RepositorioPropietarios rep = new RepositorioPropietarios();
        var listaIDs = rep.ObtenerListaIDsPropietarios();
        ViewBag.ListaIDs = listaIDs;
        return View();
    }

    // POST: inmuebles/Crear
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Crear(Inmueble i)
    {
        try
        {
            RepositorioInmuebles repo = new RepositorioInmuebles();
            repo.Alta(i);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: inmuebles/Edit/5
    public ActionResult Editar(int id)
    {
        RepositorioPropietarios rep = new RepositorioPropietarios();
        var listaIDs = rep.ObtenerListaIDsPropietarios();
        ViewBag.ListaIDs = listaIDs;
        RepositorioInmuebles repo = new RepositorioInmuebles();
        var inmueble = repo.GetInmueble(id);
        return View(inmueble);
    }


    // POST: inmuebles/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Editar(int id, Inmueble i)
    {
        try
        {
            RepositorioInmuebles repo = new RepositorioInmuebles();
            repo.Modificacion(i);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: inmuebles/Delete/5
    public ActionResult Eliminar(int id)
    {
        RepositorioInmuebles repo = new RepositorioInmuebles();
        var inmueble = repo.GetInmueble(id);
        return View(inmueble);
    }

    // POST: inmuebles/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Eliminar(int id, Inmueble i)
    {
        try
        {
            RepositorioInmuebles repo = new RepositorioInmuebles();
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