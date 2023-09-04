
using HelpCheck_API.Dtos;
using HelpCheck_API.Services.MasterOralHealths;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
{
    public class MasterOralHealthController : BaseController
    {
        private readonly IMasterOralHealthService _masterOralHealthService;
        public MasterOralHealthController(IMasterOralHealthService masterOralHealthService)
        {
            _masterOralHealthService = masterOralHealthService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOralHealths()
        {
            var result = await _masterOralHealthService.GetMasterOralHealthsAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateMasterOralHealth(AddMasterDataCodeAndNameDto addMasterDataDto)
        {
            var result = await _masterOralHealthService.CreateMasterOralHealthAsync(addMasterDataDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateMasterOralHealth(int id, [FromBody] EditMasterCodeAndNameDto editMasterDataDto)
        {
            editMasterDataDto.ID = id;
            var result = await _masterOralHealthService.UpdateMasterOralHealthAsync(editMasterDataDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMasterOralHealth(int id)
        {
            var result = await _masterOralHealthService.DeleteMasterOralHealthAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}