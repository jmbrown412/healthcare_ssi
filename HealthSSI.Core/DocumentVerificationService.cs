using System.Security.Cryptography;

namespace HealthSSI.Core
{
    /// <summary>
    /// Service for validating if a Document is for a patient as well as if signed by a hospital.
    /// </summary>
    public class DocumentVerificationService : IDocumentVerificationService
    {
        private readonly ISignatureService _signatureService;

        public DocumentVerificationService(ISignatureService signatureService)
        {
            _signatureService = signatureService;
        }

        /// <summary>
        /// Validate a document for hospital signature and patient accuracy
        /// </summary>
        /// <param name="patient"></param>
        /// <param name="document"></param>
        /// <param name="signedMessage"></param>
        /// <param name="publicKey"></param>
        /// <returns>DocumentValidationResult</returns>
        public DocumentValidationResult ValidateDocument(Patient patient, Document document, string signedMessage, string publicKey)
        {
            var forPateient = document.PatientId == patient.Id;
            RSACryptoServiceProvider importedKey = _signatureService.ImportPublicKey(publicKey);
            var hospitalSigned = _signatureService.VerifySignature(document.ToJson(), signedMessage, importedKey.ExportParameters(false));
            return new DocumentValidationResult(forPateient && hospitalSigned, hospitalSigned, forPateient);
        }
    }
}
