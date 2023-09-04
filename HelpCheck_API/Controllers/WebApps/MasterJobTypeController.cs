
using HelpCheck_API.Dtos;
using HelpCheck_API.Services.MasterJobTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
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
  
        [HttpPost]
        public async Task<IActionResult> CreateMasterJobType(AddMasterDataDto addMasterDataDto)
        {
            var result = await _masterJobTypeService.CreateMasterJobTypeAsync(addMasterDataDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateMasterJobType(int id, [FromBody] EditMasterDataDto editMasterDataDto)
        {
            editMasterDataDto.ID = id;
            var result = await _masterJobTypeService.UpdateMasterJobTypeAsync(editMasterDataDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMasterJobType(int id)
        {
            var result = await _masterJobTypeService.DeleteMasterJobTypeAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}