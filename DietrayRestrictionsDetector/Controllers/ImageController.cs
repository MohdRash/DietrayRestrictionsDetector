using DietrayRestrictionsDetector.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DietrayRestrictionsDetector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private IOCRService _ocrService;

        public ImageController(IOCRService ocrService)
        {
            _ocrService = ocrService;
        }
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            List<string> result = new List<string>();
            //  string text;
            string concatenatedText;

            if (file != null)
            {

                using (Stream imageStream = file.OpenReadStream())
                {
                    var detectedText = await _ocrService.DetectTextInImageAsync(imageStream);

                    concatenatedText = string.Join(" ", detectedText);
                }

                foreach (var item in Helpers.Helpers.restrictionsList)
                {
                    if (concatenatedText.Contains(item))
                    {
                        result.Add(item);
                    }

                }

                return Ok(result.ToArray());

            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}