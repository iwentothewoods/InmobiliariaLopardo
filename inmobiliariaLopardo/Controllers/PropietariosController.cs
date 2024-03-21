using System.Windows.Markup;
using Microsoft.AspNetCore.Mvc;
using inmobiliariaLopardo.Models;

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

    /*public IActionResult Editar(int id)
    {
        if (id > 0)
        {
            RepositorioPropietarios rp = new RepositorioPropietarios();
            var propietario = rp.GetPropietario(id);
            return View(propietario);
        }
        else
        {
            return View();
        }
    }*/

    /*public IActionResult Guardar(Propietario propietario)
    {
        RepositorioPropietarios rp = new RepositorioPropietarios();
        rp.GuardarPropietario(propietario);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Modificar(Propietario propietario)
    {
        RepositorioPropietarios rp = new RepositorioPropietarios();
        rp.ModificarPropietario(propietario);
        return RedirectToAction(nameof(Index));
    }*/

     // GET: Propietarios/Crear
        public ActionResult Crear()
        {
            return View();
        }

        // POST: Propietarios/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Propietario p)
        {
            try
            {
                RepositorioPropietarios repo = new RepositorioPropietarios();
                repo.Alta(p);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Edit/5
        public ActionResult Editar(int id)
        {
            RepositorioPropietarios repo = new RepositorioPropietarios();
            var propietario = repo.GetPropietario(id);
            return View(propietario);
        }


        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Propietario p)
        {
            try
            {
                RepositorioPropietarios repo = new RepositorioPropietarios();
                repo.Modificacion(p);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietarios/Delete/5
        public ActionResult Eliminar(int id)
        {
            RepositorioPropietarios repo = new RepositorioPropietarios();
            var propietario = repo.GetPropietario(id);
            return View(propietario);
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, Propietario p)
        {
            try
            {
                RepositorioPropietarios repo = new RepositorioPropietarios();
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



