using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSSI.Data.Entities
{
    [Table("InsuranceCompanies")]
    public class InsuranceCo
    {
        public InsuranceCo(string name)
        {
            Name = name;
            CreatedAtUTC = DateTime.UtcNow;
        }

        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAtUTC { get; set; }
    }
}
