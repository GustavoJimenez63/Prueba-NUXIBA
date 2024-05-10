using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Empleados
    {
        public Usuarios userid { get; set; }
        public float Sueldo { get; set; }
        public DateTime FechaIngreso { get; set; }

    }
}
