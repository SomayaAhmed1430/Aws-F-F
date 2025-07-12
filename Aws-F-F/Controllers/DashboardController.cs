using Aws_F_F.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Aws_F_F.Controllers
{
    [AdminOnly]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
