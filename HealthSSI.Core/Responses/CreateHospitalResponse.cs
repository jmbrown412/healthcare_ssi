using HealthSSI.Data.Entities;
using System;

namespace HealthSSI.Core.Responses
{
    public class CreateHospitalResponse : BaseResponse
    {
        public CreateHospitalResponse() : base(true) { }

        public CreateHospitalResponse(
            Hospital hospital
        ) : base(true)
        {
            Id = hospital.Id;
            Name = hospital.Name;
            CreatedAtUTC = hospital.CreatedAtUTC;
            PublicKey = hospital.PublicKey;
        }

        public CreateHospitalResponse(string error) : base(false, error)
        {
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public string PublicKey { get; set; }
    }
}
