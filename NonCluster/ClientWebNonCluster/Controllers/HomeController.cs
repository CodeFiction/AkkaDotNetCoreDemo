using Microsoft.AspNetCore.Mvc;

namespace ClientWebNonCluster.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
