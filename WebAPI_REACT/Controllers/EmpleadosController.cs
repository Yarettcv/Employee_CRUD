using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_REACT.Models;

namespace WebAPI_REACT.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly MiDbContext _context;
        public EmpleadosController(MiDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        //[Route("RegistrarEmpleado")]
        public ActionResult<Empleado> RegistrarEmpleado(Empleado empleado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Empleados.Add(empleado);
            _context.SaveChanges();

            return CreatedAtAction(nameof(RegistrarEmpleado), new { id = empleado.IdEmpleado }, empleado);
        }

        [HttpGet]
        //[Route("ListaEmpleados")]
        public ActionResult<IEnumerable<Empleado>> ObtenerEmpleados()
        {
            return _context.Empleados.ToList();
        }

        [HttpGet("{id}")]
        //[Route("ObtenerEmpleado")]
        public ActionResult<Empleado> ObtenerEmpleado(int id)
        {
            var product = _context.Empleados.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpPut("{id}")]
        //[Route("Actualizar")]
        public IActionResult ActualizarEmpleado(int id, Empleado empleado)
        {
            var existingEmp = _context.Empleados.Find(id);

            if (existingEmp == null)
            {
                return NotFound();
            }

            // Actualizar solo las propiedades que no estén vacias o sean nulas
            if (!string.IsNullOrEmpty(empleado.Nombre))
                existingEmp.Nombre = empleado.Nombre;

            if (!string.IsNullOrEmpty(empleado.Puesto))
                existingEmp.Puesto = empleado.Puesto;

            if (!string.IsNullOrEmpty(empleado.Sede))
                existingEmp.Sede = empleado.Sede;

            if (empleado.FechaBaja != null)
                existingEmp.FechaBaja = empleado.FechaBaja;
            
            _context.SaveChanges();

            return Ok(new { message = "El empleado ha sido actualizado con éxito.", empleado = existingEmp });
        }

        [HttpDelete("{id}")]
        //[Route("Eliminar")]
        public IActionResult EliminarEmpleado(int id)
        {
            var ExistEmpleado = _context.Empleados.Find(id);
            if (ExistEmpleado == null)
            {
                return NotFound();
            }

            _context.Empleados.Remove(ExistEmpleado);
            _context.SaveChanges();

            return Ok(new { message = "El empleado ha sido eliminado con éxito.", empleado = ExistEmpleado });
        }
    }
}
