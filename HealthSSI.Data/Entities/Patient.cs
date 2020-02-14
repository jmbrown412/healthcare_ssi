using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSSI.Data.Entities
{
    [Table("Patients")]
    public class Patient
    {
        public Patient() { }

        public Patient(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CreatedAtUTC = DateTime.UtcNow;
        }

        [Key]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public DateTime CreatedAtUTC { get; set; }
    }
}
