namespace AbleEaseDomain.Entities
{
    public class ReceivedMessages
    {
        // Composite Key: SenderSSN + Id
        public Guid ReceivedSSN { get; set; }
        public int Id { get; set; }

        public Guid SenderSSN { get; set; }


        public string? Subject { get; set; }
        public string? Body { get; set; }
        public DateTime SentDate { get; set; }

        // Required parent (identifying relationship)
        public User Receiver { get; set; } = null!;

        // Non-identifying relationship


    }

    // ============================================
    // JOIN TABLE: CENTER-THERAPIES
    // ============================================
 
}