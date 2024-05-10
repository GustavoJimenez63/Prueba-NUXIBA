using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;

namespace Prueba_2_NUXIBA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuarios> oUsuario = new List<Usuarios>();
            Datos_Usuarios lista = new Datos_Usuarios();
            oUsuario = lista.obtenerUsuarios();
            return Json(new { data = oUsuario, Estado = true }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult Guardar(Usuarios obj)
        {
            object resultado;
            string mensaje = string.Empty;

            if (obj.userid == 0)
            {
                resultado = new Datos_Usuarios().Registrar(obj, out mensaje);
            }
            else
            {
                resultado = new Datos_Usuarios().Editar(obj, out mensaje);
            }

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

    }
}