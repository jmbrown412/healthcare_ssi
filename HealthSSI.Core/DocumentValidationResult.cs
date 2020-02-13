namespace HealthSSI.Core
{
    /// <summary>
    /// POCO with results for Document Validation
    /// </summary>
    public class DocumentValidationResult
    {
        public DocumentValidationResult(bool sucess, bool hospitalSigned, bool forPatient)
        {
            Success = sucess;
            HospitalSigned = hospitalSigned;
            ForPatient = forPatient;
        }

        public bool Success { get; set; }
        public bool HospitalSigned { get; set; }
        public bool ForPatient { get; set; }
    }
}
