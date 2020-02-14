using HealthSSI.Data;
using System;

namespace HealthSSI.Core.Responses
{
    public class CreateDocResponse : BaseResponse
    {
        public CreateDocResponse(
            Document doc
        ) : base(true)
        {
            Id = doc.Id;
            CreatedAtUTC = doc.CreatedAtUtc;
            PatientId = doc.PatientId;
        }

        public CreateDocResponse(string error) : base(false, error)
        {
        }

        public int Id { get; set; }
        public DateTime CreatedAtUTC { get; set; }
        public long PatientId { get; set; }
    }
}
