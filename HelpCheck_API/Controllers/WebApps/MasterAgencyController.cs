
using HelpCheck_API.Dtos;
using HelpCheck_API.Services.MasterAgencies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
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

        [HttpPost]
        public async Task<IActionResult> CreateMasterAgency(AddMasterDataDto addMasterDataDto)
        {
            var result = await _masterAgencyService.CreateMasterAgencyAsync(addMasterDataDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateMasterAgency(int id, [FromBody] EditMasterDataDto editMasterDataDto)
        {
            editMasterDataDto.ID = id;
            var result = await _masterAgencyService.UpdateMasterAgencyAsync(editMasterDataDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMasterAgency(int id)
        {
            var result = await _masterAgencyService.DeleteMasterAgencyAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}