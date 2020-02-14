namespace HealthSSI.Core.Requests
{
    public class CreateDocRequest
    {
        public CreateDocRequest() { }

        public CreateDocRequest(int patientId, int hospitalId, string signedMessage)
        {
            PatientId = patientId;
            HospitalId = hospitalId;
            SignedMessage = signedMessage;
        }

        public int PatientId { get; set; }
        public int HospitalId { get; set; } 
        public string SignedMessage { get; set; }
    }
}
