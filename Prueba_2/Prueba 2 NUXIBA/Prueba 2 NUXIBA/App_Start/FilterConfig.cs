using System.Web;
using System.Web.Mvc;

namespace Prueba_2_NUXIBA
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
