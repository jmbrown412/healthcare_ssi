namespace HealthSSI.Core.Requests
{
    public class CreatePatientRequest
    {
        public CreatePatientRequest(){}

        public CreatePatientRequest(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
