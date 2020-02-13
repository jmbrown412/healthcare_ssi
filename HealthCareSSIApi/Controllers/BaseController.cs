using HealthSSI.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareSSIApi.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected ActionResult GetApiResponse(BaseResponse response)
        {
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}