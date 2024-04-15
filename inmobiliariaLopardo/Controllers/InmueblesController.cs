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

    public IActionResult Index(int? tipoId, int? dispId)
    {
        RepositorioInmuebles rp = new RepositorioInmuebles();
        IList<Inmueble> lista;

        if (tipoId.HasValue)
        {
            lista = rp.GetInmueblesPorTipo(tipoId.Value);
        }else if(dispId.HasValue)
        {
            lista = rp.GetInmueblesPorDisponibilidad(dispId.Value - 1);
        }
        else
        {
            lista = rp.GetInmuebles();
        }

        return View(lista);
    }



    // GET: Inmuebles/Crear
    public ActionResult Crear()
    {
        RepositorioPropietarios rep = new RepositorioPropietarios();
        var listaPropietarios = rep.GetPropietarios();
        ViewBag.ListaPropietarios = listaPropietarios;
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

    public ActionResult Editar(int id)
    {
        RepositorioPropietarios rep = new RepositorioPropietarios();
        var listaPropietarios = rep.GetPropietarios();
        ViewBag.ListaPropietarios = listaPropietarios;

        RepositorioInmuebles repo = new RepositorioInmuebles();
        var inmueble = repo.GetInmueble(id);
        return View(inmueble);
    }

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

        // traer la info del propietario
        if (inmueble != null && inmueble.PropietarioId != null)
        {
            RepositorioPropietarios repoPropietario = new RepositorioPropietarios();
            var propietario = repoPropietario.GetPropietario(inmueble.PropietarioId);
            if (propietario != null)
            {
                inmueble.Propietario = propietario;
            }
        }

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

    // inmuebles/Detalles/5
    [Route("inmuebles/detalles/{id}")]
    public ActionResult Detalles(int id)
    {
        RepositorioInmuebles repo = new RepositorioInmuebles();
        var inmueble = repo.GetInmueble(id);

        RepositorioPropietarios rep = new RepositorioPropietarios();
        var propietario = rep.GetPropietario(inmueble.PropietarioId);

        inmueble.Propietario = propietario;

        return View(inmueble);
    }

}