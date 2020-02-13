namespace HealthSSI.Core.Requests
{
    public class CreateHospitalRequest
    {
        public CreateHospitalRequest() { }

        public CreateHospitalRequest(string name, string publicKey)
        {
            Name = name;
            PublicKey = publicKey;
        }

        public string Name { get; set; }
        public string PublicKey { get; set; }
    }
}
