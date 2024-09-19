using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WebAPI_REACT.Custom;
using WebAPI_REACT.Models;
using WebAPI_REACT.Models.DTOs;

namespace WebAPI_REACT.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly MiDbContext _context;
        private readonly Utilidades _utilidades;
        public AccesoController(MiDbContext context, Utilidades utilidades)
        {
            _context = context;
            _utilidades = utilidades;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO objeto)
        {
            var UsuarioEncontrado = await _context.Usuarios
                                    .Where(u =>
                                    u.Correo == objeto.ClaveUser &&
                                    u.Clave == _utilidades.EncriptarSHA256(objeto.Password)
                                    ).FirstOrDefaultAsync();
            if (UsuarioEncontrado == null)
                return StatusCode(StatusCodes.Status200OK, new { isSucces = false, token = "" });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSucces = true, token = _utilidades.GenerarJWT(UsuarioEncontrado) });
        }
        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar(UsuarioDTO objeto)
        {
            var modeloUsuario = new Usuario
            {
                Nombre = objeto.Nombre,
                Clave = _utilidades.EncriptarSHA256(objeto.Clave),
                Correo = objeto.Correo,
            };

            await _context.Usuarios.AddAsync(modeloUsuario);
            await _context.SaveChangesAsync();

            if (modeloUsuario.Clave == null)
                return StatusCode(StatusCodes.Status401Unauthorized, new { idSucces = false });
            else
                return StatusCode(StatusCodes.Status201Created, new { idSucces = true });

        }
    }
}
