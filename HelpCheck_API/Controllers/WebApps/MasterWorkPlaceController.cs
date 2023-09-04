
using HelpCheck_API.Dtos;
using HelpCheck_API.Services.MasterWorkPlaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
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
        
        [HttpPost]
        public async Task<IActionResult> CreateMasterWorkPlace(AddMasterDataDto addMasterDataDto)
        {
            var result = await _masterWorkPlaceService.CreateMasterWorkPlaceAsync(addMasterDataDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateMasterWorkPlace(int id, [FromBody] EditMasterDataDto editMasterDataDto)
        {
            editMasterDataDto.ID = id;
            var result = await _masterWorkPlaceService.UpdateMasterWorkPlaceAsync(editMasterDataDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMasterWorkPlace(int id)
        {
            var result = await _masterWorkPlaceService.DeleteMasterWorkPlaceAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}