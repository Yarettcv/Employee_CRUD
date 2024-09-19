using System;
using System.Collections.Generic;

namespace WebAPI_REACT.Models;

public partial class Empleado
{
    public int IdEmpleado { get; set; }

    public string? Nombre { get; set; }

    public DateOnly? FechaAlta { get; set; }

    public DateOnly? FechaBaja { get; set; }

    public string? Puesto { get; set; }

    public string? Sede { get; set; }
}
