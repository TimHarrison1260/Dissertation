using System.Web.Mvc;

namespace AggregateWebService.Controllers
{
    /// <summary>
    /// The MVC application controller.  resents a home page
    /// to the Web Api, access to the Help page and the 
    /// Demonstration page.
    /// </summary>
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
