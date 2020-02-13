using System;

namespace HealthSSI.Core
{
    public class Patient
    {
        public string Id { get; set; }

        public Patient()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
