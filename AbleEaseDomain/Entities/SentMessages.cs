using AbleEaseDomain.Enums;

namespace AbleEaseDomain.Entities
{
    // ============================================
    // WEAK ENTITY: MESSAGE (depends on User - Sender)
    // ============================================
    public class SentMessages
    {
        // Composite Key: SenderSSN + Id
        public Guid SenderSSN { get; set; }
        public int Id { get; set; }

        public Guid ReceiverSSN { get; set; } 
        public MessageType messageType { get; set; }

        public string? Subject { get; set; }
        public string? Body { get; set; }
        public DateTime SentDate { get; set; }

        // Required parent (identifying relationship)
        public User sender { get; set; } = null!;

        // Non-identifying relationship
       
        
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}