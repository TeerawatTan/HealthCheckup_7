
using HelpCheck_API.Services.MasterJobTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.MobileApps
{
    public class MasterJobTypeController : BaseController
    {
        private readonly IMasterJobTypeService _masterJobTypeService;
        public MasterJobTypeController(IMasterJobTypeService masterJobTypeService)
        {
            _masterJobTypeService = masterJobTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetJobTypes()
        {
            var result = await _masterJobTypeService.GetMasterJobTypesAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}