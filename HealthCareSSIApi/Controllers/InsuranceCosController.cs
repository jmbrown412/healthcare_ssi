using System.Threading.Tasks;
using HealthSSI.Core;
using HealthSSI.Core.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareSSIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceCosController : BaseController
    {
        private readonly IInsuraceCoService _insCoService;

        public InsuranceCosController(
            IInsuraceCoService insCoService
        )
        {
            _insCoService = insCoService;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateInsuraceCoRequest request)
        {
            var response = await _insCoService.Create(request);
            return GetApiResponse(response);
        }
    }
}