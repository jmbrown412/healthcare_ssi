namespace HealthSSI.Core
{
    public interface IDocumentVerificationService
    {
        DocumentValidationResult ValidateDocument(Patient patient, Document document, string signedMessage, string publicKey);
    }
}
