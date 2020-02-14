 namespace HealthSSI.Core.Requests
{
    public class CreateDocRequest
    {
        public CreateDocRequest() { }

        public CreateDocRequest(long patientId, long hospitalId)
        {
            PatientId = patientId;
            HospitalId = hospitalId;
        }

        public long PatientId { get; set; }
        public long HospitalId { get; set; } 
    }
}
