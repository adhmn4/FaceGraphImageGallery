using System.Threading.Tasks;
using ImageManager.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ImageManager.API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    public class ClearImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ClearImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        [HttpDelete("deleteAll")]
        public async Task<IActionResult> DeleteAll(string imageName)
        {
            var image = await _imageService.DeleteAllImageAsync();
            return Ok(image);
        }

    }
}
