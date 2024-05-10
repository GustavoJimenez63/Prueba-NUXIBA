using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
	public class Usuarios
	{
		public int userid { get; set; }
		public string Login { get; set; }
		public string Nombre { get; set; }
		public string Paterno { get; set; }
		public string Materno { get; set; }
		public Empleados oEmpledos { get; set; }


	}
}
