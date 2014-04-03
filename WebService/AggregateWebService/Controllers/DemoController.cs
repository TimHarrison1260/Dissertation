using System.Web.Mvc;

namespace AggregateWebService.Controllers
{
    /// <summary>
    /// Controller for the Demonstration page of the MVC application
    /// </summary>
    public class DemoController : Controller
    {
        //
        // GET: /Demo/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DataSources()
        {
            return PartialView();
        }

        public ActionResult DataTypes()
        {
            return PartialView();
        }

        public ActionResult Imports()
        {
            return PartialView();
        }

        public ActionResult Windfarms()
        {
            return PartialView();
        }

        public ActionResult Errors()
        {
            return PartialView();
        }

    }
}
