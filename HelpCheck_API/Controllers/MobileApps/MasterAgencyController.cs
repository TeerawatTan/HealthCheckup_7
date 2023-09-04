
using HelpCheck_API.Services.MasterAgencies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.MobileApps
{
    public class MasterAgencyController : BaseController
    {
        private readonly IMasterAgencyService _masterAgencyService;
        public MasterAgencyController(IMasterAgencyService masterAgencyService)
        {
            _masterAgencyService = masterAgencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAgencies()
        {
            var result = await _masterAgencyService.GetMasterAgenciesAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}