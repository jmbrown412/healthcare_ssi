using HealthSSI.Core.Requests;
using HealthSSI.Core.Responses;
using HealthSSI.Data;
using HealthSSI.Data.Entities;
using System.Threading.Tasks;

namespace HealthSSI.Core
{
    public interface IDocumentService
    {
        Task<CreateDocResponse> Create(CreateDocRequest request);
        Task<Document> Get(int documentId);
        DocumentValidationResult ValidateDocument(Patient patient, Document document, string signedMessage, string publicKey);
    }
}
