using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Autenticación - cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.LogoutPath = "/Usuarios/Logout";
        options.AccessDeniedPath = "/Home/Restringido";
    });

//Autorización para manejar permisos
//Se puede añadir uno para empleados, superadmin, etc
builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Administrador", policy => policy.RequireClaim(ClaimTypes.Role, "Administrador"));
        options.AddPolicy("Empleado", policy => policy.RequireClaim(ClaimTypes.Role, "Empleado"));
    });
    
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); //Archivos CSS, JS

app.UseRouting();

app.UseAuthentication(); //Autenticación, va primero o no anda
app.UseAuthorization(); //Autorización

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
