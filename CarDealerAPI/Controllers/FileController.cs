using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CarDealerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    [ResponseCache(Duration = 1200, VaryByQueryKeys = new[] { "fileName"})]   
    public class FileController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetFile([FromQuery] string fileName)
        {
            var rootPath = Directory.GetCurrentDirectory();

            var filePath = $"{rootPath}/FileToDownloadForLoggedUser/{fileName}";

            var isExist = System.IO.File.Exists(filePath);

            if (!isExist)
            {
                return NotFound();
            }

            var contentProvider = new FileExtensionContentTypeProvider();
            contentProvider.TryGetContentType(filePath, out string fileType);

            var fileContent = System.IO.File.ReadAllBytes(filePath);

            return File(fileContent, fileType, fileName);
        }

        [HttpPost]
        public ActionResult Upload([FromForm] IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var rootPath = Directory.GetCurrentDirectory();
                var faileName = file.FileName;
                var fullPath = $"{rootPath}/FileToDownloadForLoggedUser/{faileName}";

                using (var source = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(source);

                }
                return Ok();
            }
            return BadRequest();
        }
    }
}
