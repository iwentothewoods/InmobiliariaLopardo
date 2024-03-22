using System.Windows.Markup;
using Microsoft.AspNetCore.Mvc;
using inmobiliariaLopardo.Models;

namespace inmobiliariaLopardo.Controllers;

public class InquilinosController : Controller
{
    private readonly ILogger<InquilinosController> _logger;

    public InquilinosController(ILogger<InquilinosController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        RepositorioInquilinos rp = new RepositorioInquilinos();
        var lista = rp.GetInquilinos();
        return View(lista);
    }

    public ActionResult Crear()
        {
            return View();
        }

    [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(Inquilino p)
        {
            try
            {
                RepositorioInquilinos repo = new RepositorioInquilinos();
                repo.Alta(p);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    
    public ActionResult Editar(int id)
        {
            RepositorioInquilinos repo = new RepositorioInquilinos();
            var inquilino = repo.GetInquilino(id);
            return View(inquilino);
        }

    [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(int id, Inquilino p)
        {
            try
            {
                RepositorioInquilinos repo = new RepositorioInquilinos();
                repo.Modificacion(p);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    public ActionResult Eliminar(int id)
        {
            RepositorioInquilinos repo = new RepositorioInquilinos();
            var inquilino = repo.GetInquilino(id);
            return View(inquilino);
        }

    [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id, Inquilino p)
        {
            try
            {
                RepositorioInquilinos repo = new RepositorioInquilinos();
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