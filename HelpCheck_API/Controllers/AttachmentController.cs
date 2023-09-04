using HelpCheck_API.Dtos.Attachments;
using HelpCheck_API.Services.Attachments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttachmentController : ControllerBase
    {
        private readonly IAttachmentService _attachmentService;
        private readonly IHostingEnvironment _environment;
        public AttachmentController(IAttachmentService attachmentService, IHostingEnvironment environment)
        {
            _attachmentService = attachmentService;
            _environment = environment;
        }

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadFile([FromForm] UploadImageDto req)
        {
            var result = await _attachmentService.UploadImageAsync(req);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpGet("DownloadImage/{id}")]
        public IActionResult DownloadFile(string id)
        {
            try
            {
                string path = Path.Combine(_environment.ContentRootPath, "UploadedFiles");
                var file = Directory.GetFiles(path).Where(w => w.Contains(id)).ToArray();
                if (!file.Any())
                    return BadRequest($"Image id : `{id}` not found.");
                return File(new FileStream(file[0], FileMode.Open), "image/jpg", "image1.jpg");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
