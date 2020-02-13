namespace HealthSSI.Core.Requests
{
    public class CreateInsuraceCoRequest
    {
        public CreateInsuraceCoRequest() { }

        public CreateInsuraceCoRequest(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
