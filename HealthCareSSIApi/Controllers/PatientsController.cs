using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthSSI.Core;
using HealthSSI.Core.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareSSIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : BaseController
    {
        private readonly IPatientService _patientService;

        public PatientsController(
            IPatientService patientService
        )
        {
            _patientService = patientService;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreatePatientRequest request)
        {
            var response = await _patientService.Create(request);
            return GetApiResponse(response);
        }
    }
}