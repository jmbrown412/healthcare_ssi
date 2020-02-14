using HealthSSI.Core.Requests;
using HealthSSI.Core.Responses;
using HealthSSI.Data;
using HealthSSI.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HealthSSI.Core
{
    /// <summary>
    /// Service for validating if a Document is for a patient as well as if signed by a hospital.
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly ISignatureService _signatureService;
        private readonly SSIDbContext _dbContext;

        public DocumentService(ISignatureService signatureService, SSIDbContext dbContext)
        {
            _signatureService = signatureService;
            _dbContext = dbContext;
        }

        public async Task<CreateDocResponse> Create(CreateDocRequest request)
        {
            try
            {
                var hospital = await _dbContext.Hospitals.FirstOrDefaultAsync(h => h.Id == request.HospitalId);
                var doc = new Document(request.PatientId, hospital);
                
                _dbContext.Documents.Add(doc);
                
                await _dbContext.SaveChangesAsync();
                return new CreateDocResponse(doc, hospital);
            }
            catch (Exception ex)
            {
                // log details somewhere. i.e. inner exception, stacktrace, etc...
                return new CreateDocResponse("There was an error creating a new Document");
            }
        }

        public async Task<SignDocResponse> SignDoc(SignDocRequest request)
        {
            try
            {
                var doc = await _dbContext.Documents.FirstOrDefaultAsync(d => d.Id == request.Document.Id);
                var documentSignedMessage = new DocumentSignedMessage(doc, request.SignedMessage);
                _dbContext.DocumentSignedMessages.Add(documentSignedMessage);
                await _dbContext.SaveChangesAsync();
                return new SignDocResponse(doc.Id, documentSignedMessage.Id);
            }
            catch (Exception ex)
            {
                // log details somewhere. i.e. inner exception, stacktrace, etc...
                return new SignDocResponse("There was an error signing a Document");
            }
        }

        public async Task<Document> Get(long documentId)
        {
            try
            {
                return await _dbContext.Documents.FirstOrDefaultAsync(doc => doc.Id == documentId);
            }
            catch (Exception ex)
            {
                // log details somewhere. i.e. inner exception, stacktrace, etc...
                throw;
            }
        }

        /// <summary>
        /// Validate a document for hospital signature and patient accuracy
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="document"></param>
        /// <param name="signedMessage"></param>
        /// <param name="publicKey"></param>
        /// <returns>DocumentValidationResult</returns>
        public DocumentValidationResult ValidateDocument(long patientId, Document document, string signedMessage, string publicKey)
        {
            var forPateient = document.PatientId == patientId;
            RSACryptoServiceProvider importedKey = _signatureService.ImportPublicKey(publicKey);
            var hospitalSigned = _signatureService.VerifySignature(document.ToJson(), signedMessage, importedKey.ExportParameters(false));
            return new DocumentValidationResult(forPateient && hospitalSigned, hospitalSigned, forPateient);
        }
    }
}
