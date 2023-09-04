using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Attachments;
using HelpCheck_API.Helpers;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IHostingEnvironment _environment;
        public AttachmentService(IHostingEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<ResultResponse> UploadImageAsync(UploadImageDto req)
        {
            try
            {
                string path = "", newFileName = "";
                if (req != null && req.Image != null && req.Image.Length > 0)
                {
                    newFileName = Guid.NewGuid().ToString();
                    string fileExtension = Path.GetExtension(req.Image.FileName);
                    path = Path.Combine(_environment.ContentRootPath, "UploadedFiles");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, newFileName + fileExtension), FileMode.Create))
                    {
                        await req.Image.CopyToAsync(fileStream);
                    }
                }

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = newFileName
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Message
                };
            }
        }
    }
}
