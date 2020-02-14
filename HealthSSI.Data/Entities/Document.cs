using HealthSSI.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSSI.Data
{
    [Table("Documents")]
    public class Document : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public virtual Patient Patient { get; set; }
        [ForeignKey("PatientId")]
        public long PatientId { get; set; }
        public virtual Hospital Hospital { get; set; }
        [ForeignKey("Hospital")]
        public long HospitalId { get; set; }

        public Document() { }

        public Document(long patientId, Hospital hospital)
        {
            CreatedAtUtc = DateTime.UtcNow;
            PatientId = patientId;
            Hospital = hospital;
        }
    }
}