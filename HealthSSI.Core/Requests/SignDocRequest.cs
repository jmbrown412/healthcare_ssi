using HealthSSI.Data;

namespace HealthSSI.Core.Requests
{
    public class SignDocRequest
    {
        public SignDocRequest(Document document, string signedMessage)
        {
            Document = document;
            SignedMessage = signedMessage;
        }

        public Document Document { get; set; }
        public string SignedMessage { get; set; }
    }
}
