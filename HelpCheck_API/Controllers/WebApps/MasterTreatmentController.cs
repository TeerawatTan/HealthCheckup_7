
using HelpCheck_API.Dtos;
using HelpCheck_API.Services.MasterTreatments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpCheck_API.Controllers.WebApps
{
    public class MasterTreatmentController : BaseController
    {
        private readonly IMasterTreatmentService _masterTreatmentService;
        public MasterTreatmentController(IMasterTreatmentService masterTreatmentService)
        {
            _masterTreatmentService = masterTreatmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTreatments()
        {
            var result = await _masterTreatmentService.GetMasterTreatmentsAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateMasterTreatment(AddMasterDataDto addMasterDataDto)
        {
            var result = await _masterTreatmentService.CreateMasterTreatmentAsync(addMasterDataDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateMasterTreatment(int id, [FromBody] EditMasterDataDto editMasterDataDto)
        {
            editMasterDataDto.ID = id;
            var result = await _masterTreatmentService.UpdateMasterTreatmentAsync(editMasterDataDto);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMasterTreatment(int id)
        {
            var result = await _masterTreatmentService.DeleteMasterTreatmentAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result.Data);
            }
            return Ok(result.Data);
        }
    }
}