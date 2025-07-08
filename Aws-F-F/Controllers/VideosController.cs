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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            
            var video = await _context.Videos.FindAsync(id);
            if (video == null) return NotFound();

            // حذف الفيديو من السيرفر
            if (!string.IsNullOrEmpty(video.VideoPath))
            {
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, video.VideoPath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            _context.Videos.Remove(video);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // edit (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var video = await _context.Videos.FindAsync(id);
            if (video == null) return NotFound();
            return View(video);
        }

        // edit (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Video video, IFormFile? VideoFile)
        {
            if (id != video.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingVideo = await _context.Videos.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);
                    if (existingVideo == null) return NotFound();

                    // لو فيه فيديو جديد، ارفع الجديد واحذف القديم
                    if (VideoFile != null && VideoFile.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(existingVideo.VideoPath))
                        {
                            string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, existingVideo.VideoPath.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                                System.IO.File.Delete(oldFilePath);
                        }

                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "videos");
                        Directory.CreateDirectory(uploadsFolder);
                        string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(VideoFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await VideoFile.CopyToAsync(stream);
                        }

                        video.VideoPath = "/videos/" + uniqueFileName;
                    }
                    else
                    {
                        // لو مفيش فيديو جديد، نحتفظ بالقديم
                        video.VideoPath = existingVideo.VideoPath;
                    }

                    video.UploadDate = DateTime.Now;

                    _context.Update(video);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Videos.Any(e => e.Id == video.Id)) return NotFound();
                    else throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(video);
        }

    }
}
