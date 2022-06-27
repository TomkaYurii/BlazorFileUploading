using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadsController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileUploadsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public async Task<IActionResult> Post ([FromForm] IFormFile image)
        {
            if (image == null || image.Length == 0) return BadRequest("Upload any Image");

            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);

            string[] allow = { ".jpg", ".png" };

            if (!allow.Contains(extension.ToLower())) return BadRequest("Invalid Image, Try Another");

            string  fileNameNew = $"{Guid.NewGuid()}{extension}";
            string  filePath = Path.Combine(_webHostEnvironment.ContentRootPath,"wwwroot","Files", fileNameNew);

            using (var fileStream = new FileStream(filePath,FileMode.Create,FileAccess.Write))
            {
                await image.CopyToAsync(fileStream);
            }

            return Ok($"Files/{fileNameNew}");
        }
    }
}
