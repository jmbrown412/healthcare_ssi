using System.Threading.Tasks;
using HealthSSI.Core;
using HealthSSI.Core.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareSSIApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocsController : BaseController
    {
        private readonly IDocumentService _documentService;

        public DocsController(IDocumentService docService)
        {
            _documentService = docService;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateDocRequest request)
        {
            var response = await _documentService.Create(request);
            return GetApiResponse(response);
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] long docId)
        {
            var doc = await _documentService.Get(docId);
            return Ok(doc);
        }
    }
}