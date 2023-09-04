using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Attachments;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.Attachments
{
    public interface IAttachmentService
    {
        Task<ResultResponse> UploadImageAsync(UploadImageDto req);
    }
}
