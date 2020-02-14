namespace HealthSSI.Core.Responses
{
    public class SignDocResponse : BaseResponse
    {
        public SignDocResponse(long docId, long documentSignedMessageId) : base(true)
        {
            DocId = docId;
            DocumentSignedMessageId = documentSignedMessageId;
        }

        public SignDocResponse(string error) : base(false, error)
        {
        }

        public long DocId { get; set; }
        public long DocumentSignedMessageId { get; set; }
    }
}
