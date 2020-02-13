using System.Threading.Tasks;
using HealthSSI.Core;
using HealthSSI.Core.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareSSIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalsController : BaseController
    {
        private readonly IHospitalService _hospitalService;

        public HospitalsController(
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
    }
}