using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSSI.Data.Entities
{
    [Table("Hospital")]
    public class Hospital
    {
        public Hospital(string name, string publicKey)
        {
            Name = name;
            PublicKey = publicKey;
            CreatedAtUTC = DateTime.UtcNow;
        }

        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string PublicKey { get; set; }
        public DateTime CreatedAtUTC { get; set; }
    }
}
