using System.Threading.Tasks;
using Aws_F_F.Data;
using Aws_F_F.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aws_F_F.Controllers
{
    public class VideosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VideosController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var videos = await _context.Videos.ToListAsync();
            return View(videos);
        }


        // create (GET)
        public IActionResult Create()
        {
            return View();
        }

        // create (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Video video, IFormFile VideoFile)
        {
            if (ModelState.IsValid) 
            {
                if (VideoFile != null && VideoFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "videos");
                    Directory.CreateDirectory(uploadsFolder); // لو مجاش يعمل الفولدر
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(VideoFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await VideoFile.CopyToAsync(fileStream);
                    }

                    video.VideoPath = "/videos/" + uniqueFileName;
                }

                video.UploadDate = DateTime.Now;
                _context.Add(video);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(video);
        }


    }
}
