using HealthSSI.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthSSI.Core.Responses
{
    public class CreatePatientResponse : BaseResponse
    {
        public CreatePatientResponse(
            Patient patient
        ) : base(true)
        {
            FirstName = patient.FirstName;
            LastName = patient.LastName;
            Email = patient.Email;
            CreatedAtUTC = patient.CreatedAtUTC;
        }

        public CreatePatientResponse(string error) : base(false, error)
        {
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAtUTC { get; set; }
    }
}
