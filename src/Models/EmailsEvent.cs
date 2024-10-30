using System;

namespace pcnintegrationservices.Models
{
    public class EmailsEvent
    {
        public int Id { get; set; } 
        public string Event { get; set; } = String.Empty;
        public string Recipient { get; set; } = String.Empty;
        public string MessageId { get; set; } = String.Empty;
        public DateTime Timestamp { get; set; }
    }
}
