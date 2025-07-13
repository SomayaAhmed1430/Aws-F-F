using System.Diagnostics;
using Aws_F_F.Models;
using Aws_F_F.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Aws_F_F.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Videos()
        {
            var videos = _context.Videos.OrderByDescending(v => v.UploadDate).ToList();
            return View(videos);
        }

        public IActionResult VideoDetails(int id)
        {
            var video = _context.Videos.FirstOrDefault(a => a.Id == id);
            if (video == null)
            {
                return NotFound();
            }
            return View(video);
        }

        public IActionResult Articles()
        {
            var articles = _context.Articles.OrderByDescending(a => a.PublishDate).ToList();
            return View(articles);
        }

        public IActionResult ArticleDetails(int id)
        {
            var article = _context.Articles.FirstOrDefault(a => a.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }


        // GET
        [HttpGet]
        public IActionResult Admin()
        {
            return View();
        }

        // POST
        [HttpPost]
        public IActionResult Admin(string password)
        {
            if (password == "myAdmin123")
            {
                HttpContext.Session.SetString("IsAdmin", "true"); // ✅ نحط علامة الدخول
                return RedirectToAction("Index", "Articles");
            }
            else
            {
                ViewBag.Error = "كلمة المرور غير صحيحة";
                return View();
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
