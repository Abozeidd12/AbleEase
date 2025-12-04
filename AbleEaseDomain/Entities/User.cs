using System.ComponentModel.DataAnnotations;

namespace AbleEaseDomain.Entities
{
    // ============================================
    // BASE USER CLASS
    // ============================================
    public abstract class User
    {
        [Key]
        public Guid SSN { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? ContactInfo { get; set; }

        // WEAK ENTITY: Messages sent by this user
        public List<SentMessages> SentMessages { get; set; } = new List<SentMessages>();

        // Messages received (non-identifying relationship)
        public List<ReceivedMessages> ReceivedMessages { get; set; } = new List<ReceivedMessages>();
    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}