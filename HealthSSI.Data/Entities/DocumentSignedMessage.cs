using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthSSI.Data.Entities
{
    [Table("DocumentSignedMessages")]
    public class DocumentSignedMessage
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public virtual Document Document { get; set; }
        [ForeignKey("Document")]
        public int DocumentId { get; set; }
        public string SignedMesage { get; set; }

        public DocumentSignedMessage() { }

        public DocumentSignedMessage(Document document, string signedMessage)
        {
            CreatedAtUtc = DateTime.UtcNow;
            Document = document;
            SignedMesage = signedMessage;
        }
    }
}
