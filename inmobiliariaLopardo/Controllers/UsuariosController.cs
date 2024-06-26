using System.Windows.Markup;
using Microsoft.AspNetCore.Mvc;
using inmobiliariaLopardo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using MySql.Data.MySqlClient;

namespace inmobiliariaLopardo.Controllers;

[Authorize]
public class UsuariosController : Controller
{

    private readonly IConfiguration configuration;
    private readonly IWebHostEnvironment environment;
    private readonly ILogger<UsuariosController> _logger;


    public UsuariosController(ILogger<UsuariosController> logger, IConfiguration configuration, IWebHostEnvironment environment)
    {
        _logger = logger;
        this.configuration = configuration;
        this.environment = environment;
    }

    [Authorize(Roles = "Administrador")]
    public IActionResult Index()
    {
        RepositorioUsuarios ru = new RepositorioUsuarios();
        var lista = ru.GetUsuarios();
        return View(lista);
    }

    // GET: Usuarios/Crear
    [Authorize("Administrador")]
    public ActionResult Crear()
    {
        return View();
    }

    // POST: Usuarios/Crear
    [Authorize("Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Crear(Usuario u)
    {
        if (!ModelState.IsValid)
            return View(u);

        try
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: u.Clave,
                            salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8));
            u.Clave = hashed;


            if (u.AvatarFile != null)
            {
                string wwwPath = environment.WebRootPath;
                string path = Path.Combine(wwwPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = "avatar_" + Guid.NewGuid().ToString() + Path.GetExtension(u.AvatarFile.FileName);
                string pathCompleto = Path.Combine(path, fileName);
                u.Avatar = Path.Combine("/Uploads", fileName);

                using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                {
                    u.AvatarFile.CopyTo(stream);
                }
            }

            RepositorioUsuarios repo = new RepositorioUsuarios();
            repo.Alta(u);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error al guardar el usuario: " + ex.Message);
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View(u);
        }
    }

    // GET: Usuarios/Edit/5
    [Authorize(Roles = "Administrador")]
    public ActionResult Editar(int id)
    {
        RepositorioUsuarios repo = new RepositorioUsuarios();
        var Usuario = repo.GetUsuario(id);
        return View(Usuario);
    }


    // POST: Usuarios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrador")]
    public ActionResult Editar(int id, Usuario u)
    {
        try
        {
            RepositorioUsuarios repo = new RepositorioUsuarios();
            repo.Modificacion(u);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: Usuarios/Delete/5
    [Authorize(Roles = "Administrador")]
    public ActionResult Eliminar(int id)
    {
        RepositorioUsuarios repo = new RepositorioUsuarios();
        var Usuario = repo.GetUsuario(id);
        return View(Usuario);
    }

    // POST: Usuarios/Delete/5
    [Authorize(Roles = "Administrador")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Eliminar(int id, Usuario u)
    {
        try
        {
            RepositorioUsuarios repo = new RepositorioUsuarios();
            repo.Baja(id);
            TempData["Mensaje"] = "Eliminación realizada correctamente";
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    [Route("Usuarios/detalles/{id}")]
    [Authorize(Roles = "Administrador")]
    public ActionResult Detalles(int id)
    {
        RepositorioUsuarios repo = new RepositorioUsuarios();
        var Usuario = repo.GetUsuario(id);

        return View(Usuario);
    }

    [AllowAnonymous]
    // GET: Usuarios/Login/
    public ActionResult Login(string returnUrl)
    {
        TempData["returnUrl"] = returnUrl;
        var usuario = new Usuario();
        return View(usuario);
    }

    // POST: Usuarios/Login/
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(Usuario usuario)
    {
        RepositorioUsuarios repositorio = new RepositorioUsuarios();
        try
        {
            var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
            if (ModelState.IsValid)
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: usuario.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 1000,
                    numBytesRequested: 256 / 8));

                var e = repositorio.ObtenerPorEmail(usuario.Email);
                if (e == null || e.Clave != hashed)
                {
                    ViewBag.ErrorMessage = "El email o la clave no son correctos";
                    ModelState.AddModelError("", "El email o la clave no son correctos");
                    TempData["returnUrl"] = returnUrl;
                    return View();
                }

                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, e.Email),
                new Claim("FullName", e.Nombre + " " + e.Apellido),
                new Claim(ClaimTypes.Role, e.RolNombre),
            };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                TempData.Remove("returnUrl");
                return Redirect(returnUrl);
            }
            TempData["returnUrl"] = returnUrl;
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    public ActionResult Perfil()
    {
        RepositorioUsuarios repo = new RepositorioUsuarios();
        var roles = repo.getEnumRol();
        ViewBag.roles = roles;
        ViewBag.Titulo = "Mi Perfil";
        var u = repo.ObtenerPorEmail(User.Identity.Name);
        return View("Detalles", u);
    }

    public ActionResult Configuracion()
    {
        RepositorioUsuarios repo = new RepositorioUsuarios();
        var roles = repo.getEnumRol();
        ViewBag.roles = roles;
        ViewBag.Titulo = "Configuración";
        var u = repo.ObtenerPorEmail(User.Identity.Name);
        return View("Editar", u);
    }

    [AllowAnonymous]
    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );
        return RedirectToAction("Index", "Home");
    }

    /* CAMBIAR AVATAR Y CONTRASEÑA */

    // GET: Usuarios/CambiarClave
    public IActionResult CambiarClave()
    {
        return View();
    }

    // POST: Usuarios/CambiarClave
    [HttpPost]
    [ValidateAntiForgeryToken]
    // [Route("Usuarios/CambiarClave")]
    public IActionResult CambiarClave(string clave)
    {
        try
        {
            // Obtener el usuario autenticado
            string emailUsuario = User.Identity.Name;
            RepositorioUsuarios repo = new RepositorioUsuarios();
            Usuario usuarioDB = repo.ObtenerPorEmail(emailUsuario);

            if (usuarioDB == null)
            {
                return NotFound();
            }

            // Hash de la nueva contraseña
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: clave,
                            salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8));

            // Cambiar la clave del usuario
            repo.CambiarClave(usuarioDB, hashed);

            return RedirectToAction("Index", "Usuarios");
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = "Error al cambiar la clave: " + ex.Message;
            return View();
        }
    }


// Metodo GET para mostrar la vista
[HttpGet]
public IActionResult CambiarAvatar()
{
    return View();
}

// Metodo POST para modificar o eliminar el avatar
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult CambiarAvatar(Usuario usuario, bool eliminarAvatar)
{
    try
    {
        Console.WriteLine("Valor de eliminarAvatar: " + eliminarAvatar);
        RepositorioUsuarios repo = new RepositorioUsuarios();
        var usuarioAutenticado = repo.ObtenerPorEmail(User.Identity.Name);

        if (usuarioAutenticado == null)
        {
            TempData["Error"] = "No se pudo encontrar el usuario autenticado";
            return RedirectToAction("Perfil");
        }
        if (eliminarAvatar)
        {
            Console.WriteLine("entró al if");
            repo.EliminarAvatar(usuarioAutenticado);
            TempData["Mensaje"] = "Avatar eliminado correctamente";
        }
        else if (usuario.AvatarFile != null && usuario.AvatarFile.Length > 0)
        {
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = "avatar_" + Guid.NewGuid().ToString() + Path.GetExtension(usuario.AvatarFile.FileName);
            string pathCompleto = Path.Combine(path, fileName);
            usuario.Avatar = Path.Combine("/Uploads", fileName);

            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
            {
                usuario.AvatarFile.CopyTo(stream);
            }

            repo.CambiarAvatar(usuarioAutenticado, usuario.Avatar);

            TempData["Mensaje"] = "Avatar cambiado correctamente";
        }
        else
        {
            TempData["Mensaje"] = "No se seleccionó ningún archivo";
        }

        return RedirectToAction("Perfil");
    }
    catch (Exception ex)
    {
        TempData["Error"] = "Error al cambiar el avatar: " + ex.Message;
        return RedirectToAction("Perfil");
    }
}



}