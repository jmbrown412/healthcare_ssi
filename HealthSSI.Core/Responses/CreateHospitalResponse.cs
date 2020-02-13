using HealthSSI.Data.Entities;
using System;

namespace HealthSSI.Core.Responses
{
    public class CreateHospitalResponse : BaseResponse
    {
        public CreateHospitalResponse(
            Hospital hospital
        ) : base(true)
        {
            Name = hospital.Name;
            CreatedAtUTC = hospital.CreatedAtUTC;
            PublicKey = hospital.PublicKey;
        }

        public CreateHospitalResponse(string error) : base(false, error)
        {
        }

        public string Name { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public string PublicKey { get; set; }
    }
}
