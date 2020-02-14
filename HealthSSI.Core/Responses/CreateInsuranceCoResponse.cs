using HealthSSI.Data.Entities;
using System;

namespace HealthSSI.Core.Responses
{
    public class CreateInsuranceCoResponse : BaseResponse
    {
        public CreateInsuranceCoResponse() : base(true) { }

        public CreateInsuranceCoResponse(
            InsuranceCo insuranceCo
        ) : base(true)
        {
            Name = insuranceCo.Name;
            CreatedAtUTC = insuranceCo.CreatedAtUTC;
        }

        public CreateInsuranceCoResponse(string error) : base(false, error)
        {
        }

        public string Name { get; set; }
        public DateTime CreatedAtUTC { get; set; }
    }
}
