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
        Task<SignDocResponse> SignDoc(SignDocRequest request);
        Task<Document> Get(long documentId);
        DocumentValidationResult ValidateDocument(long patientId, Document document, string signedMessage, string publicKey);
    }
}
