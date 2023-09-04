
using HelpCheck_API.Services.MasterWorkPlaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.MobileApps
{
    public class MasterWorkPlaceController : BaseController
    {
        private readonly IMasterWorkPlaceService _masterWorkPlaceService;
        public MasterWorkPlaceController(IMasterWorkPlaceService masterWorkPlaceService)
        {
            _masterWorkPlaceService = masterWorkPlaceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkPlaces()
        {
            var result = await _masterWorkPlaceService.GetMasterWorkPlacesAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}