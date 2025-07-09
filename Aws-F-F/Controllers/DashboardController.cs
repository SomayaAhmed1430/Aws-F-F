using Microsoft.AspNetCore.Mvc;

namespace Aws_F_F.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
