using System.Threading.Tasks;
using HealthSSI.Core;
using HealthSSI.Core.Requests;
using HealthSSI.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareSSIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalController : ControllerBase
    {
        private readonly IHospitalService _hospitalService;

        public HospitalController(
            IHospitalService hospitalService
        )
        {
            _hospitalService = hospitalService;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateHospitalRequest request)
        {
            var response = await _hospitalService.Create(request);
            return GetApiResponse(response);
        }

        private ActionResult GetApiResponse(BaseResponse response)
        {
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}