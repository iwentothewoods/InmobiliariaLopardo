using System.Windows.Markup;
using Microsoft.AspNetCore.Mvc;
using inmobiliariaLopardo.Models;
using MySqlX.XDevAPI.CRUD;
using Microsoft.AspNetCore.Authorization;
using inmobiliariaLopardo.ViewModels;

namespace inmobiliariaLopardo.Controllers;

[Authorize]
public class ContratosController : Controller
{
    private readonly ILogger<ContratosController> _logger;

    public ContratosController(ILogger<ContratosController> logger)
    {
        _logger = logger;
    }



    public IActionResult Index(int? estadoId, int? inmId, DateTime? fechaIList, DateTime? fechaFList, int? diasId)
    {
        RepositorioContratos rp = new RepositorioContratos();
        IList<Contrato> lista;
        var fin = DateTime.Now;

        if (estadoId.Equals(1))
        {
            lista = rp.GetContratosActivos(fin);
        }
        else if (estadoId.Equals(2))
        {
            lista = rp.GetContratosInactivos(fin);
        }
        else if (inmId.HasValue)
        {
            lista = rp.GetPorInmuebles(inmId.Value);
        }
        else if (fechaIList.HasValue && fechaFList.HasValue)
        {
            lista = rp.GetContratosPorRango(fechaIList.Value, fechaFList.Value);
        }
        else if (diasId.HasValue)
        {
            lista = rp.GetContratosAVencer(fin.AddDays(diasId.Value));
        }
        else
        {
            lista = rp.GetContratos();
        }

        return View(lista);
    }


    public ActionResult Crear()
    {
        RepositorioInquilinos repInquilinos = new RepositorioInquilinos();
        var listaInquilinos = repInquilinos.GetInquilinos();

        RepositorioInmuebles repInmuebles = new RepositorioInmuebles();
        var listaInmuebles = repInmuebles.GetInmuebles();

        for (int i = 0; i < listaInmuebles.Count; i++)
        {
            if (listaInmuebles[i].Disponible.Equals(false))
            {
                listaInmuebles.RemoveAt(i);
                i--;
            }
        }

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
            if (c.FechaInicio <= c.FechaFin)
            {
                RepositorioContratos repo = new RepositorioContratos();
                RepositorioInmuebles repoi = new RepositorioInmuebles();
                repo.Alta(c);
                repoi.altaContrato(c);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["Mensaje"] = "Ingrese una fecha final posterior a la fecha de inicio";
                return RedirectToAction(nameof(Crear));
            }
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

    [Authorize(Roles = "Administrador")]
    public ActionResult Eliminar(int id)
    {
        RepositorioContratos repo = new RepositorioContratos();
        var contrato = repo.GetContrato(id);

        if (contrato != null && contrato.InquilinoId != null && contrato.InmuebleId != null)
        {
            RepositorioInquilinos repoInquilino = new RepositorioInquilinos();
            var inquilino = repoInquilino.GetInquilino(contrato.InquilinoId);
            if (inquilino != null)
            {
                contrato.Inquilino = inquilino;
            }

            RepositorioInmuebles repoInmuebles = new RepositorioInmuebles();
            var inmueble = repoInmuebles.GetInmueble(contrato.InmuebleId);
            if (inmueble != null)
            {
                contrato.Inmueble = inmueble;
            }
        }

        return View(contrato);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public ActionResult Eliminar(Contrato i)
    {
        try
        {
            RepositorioContratos repo = new RepositorioContratos();
            RepositorioInmuebles repoi = new RepositorioInmuebles();
            repo.Baja(i);
            repoi.bajaContrato(i);
            //TempData["Mensaje"] = "Eliminación realizada correctamente";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    /*
        public ActionResult Pagos(int id)
        {

            RepositorioContratos repo = new RepositorioContratos();
            IList<Pago> lista;
            lista = repo.GetPagos();

            var vm = new ViewModelPagoContrato
            {
                lpago = lista,
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pagos(Pago p)
        {
            try
            {
                    RepositorioContratos repo = new RepositorioContratos();
                    repo.Pagar(p);
                    return RedirectToAction(nameof(Pagos));
            }
            catch
            {
                return View();
            }
        }*/

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


    /******* PAGOS  *********/

    /*
        public ActionResult Pagos(int id)
        {

            RepositorioContratos repo = new RepositorioContratos();
            IList<Pago> lista;
            lista = repo.GetPagos();

            var vm = new ViewModelPagoContrato
            {
                lpago = lista,
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Pagos(Pago p)
        {
            try
            {
                RepositorioContratos repo = new RepositorioContratos();
                repo.Pagar(p);
                return RedirectToAction(nameof(Pagos));
            }
            catch
            {
                return View();
            }
        }
    */


[HttpGet]
public ActionResult Pagos(int id)
{
    ViewBag.ContratoId = id;
    var repo = new RepositorioContratos();
    var pagos = repo.GetPagosPorContratoId(id);
    return View(pagos);
}

[HttpPost]
public ActionResult Pagos(Pago nuevoPago)
{
    var repo = new RepositorioContratos();
    int pagoId = repo.Pagar(nuevoPago);
    if (pagoId > 0)
    {
        return RedirectToAction("Pagos", new { id = nuevoPago.ContratoId });
    }
    else
    {
        ModelState.AddModelError("", "Error al agregar el pago.");
        ViewBag.ContratoId = nuevoPago.ContratoId;
        var pagos = repo.GetPagosPorContratoId(nuevoPago.ContratoId);
        return View(pagos);
    }
}



}