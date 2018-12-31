using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ImageManager.API.Interfaces;
using Microsoft.AspNetCore.StaticFiles;

namespace ImageManager.API.Controllers
{
    [Route("[controller]")]
    [AllowAnonymous]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpGet]
        public IActionResult Get([FromBody]string imageName)
        {
            var image = _imageService.DownloadImage(imageName);
            return Ok(image);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var images = await _imageService.ListImagesAsync();
            return Ok(images);
        }

        [HttpPost("upload")]
        public async  Task<IActionResult> Upload()
        {
            // getting the file from request form
            var file = Request.Form.Files.GetFile("file");

            // checking for file type validity
            new FileExtensionContentTypeProvider().TryGetContentType(file.FileName, out var contentType);

            // if the file is currpted or wrong type return fail result
            if (file.Length <= 0 || !contentType.StartsWith("image/")) return Ok(false);

            // reading file stream
            var imageStream = file.OpenReadStream();

            // passing file stream to the service
            var result = await _imageService.UploadImageAsync(file.FileName, imageStream);
            return Ok(result != null);

        }

        [Route("delete/{imageName}"),HttpDelete]
        public async Task<IActionResult> Delete(string imageName)
        {
            var image = await _imageService.DeleteImageAsync(imageName);
            return Ok(image);
        }
    }
}
