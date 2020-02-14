using HealthSSI.Data;
using HealthSSI.Data.Entities;
using System;

namespace HealthSSI.Core.Responses
{
    public class CreateDocResponse : BaseResponse
    {
        public CreateDocResponse() : base(true) { }

        public CreateDocResponse(
            Document doc
            , Hospital hospital
        ) : base(true)
        {
            Id = doc.Id;
            CreatedAtUtc = doc.CreatedAtUtc;
            PatientId = doc.PatientId;
            HospitalId = hospital.Id;
        }

        public CreateDocResponse(string error) : base(false, error)
        {
        }

        public long Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public long PatientId { get; set; }
        public long HospitalId { get; set; }
    }
}
