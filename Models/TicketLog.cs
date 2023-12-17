namespace TicketSystemApplication.Models
{
    public class TicketLog
    {
        public int Id { get; set; }
         public int? TicketID { get; set; }
        public Ticket? Ticket { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Comments{ get; set; } = string.Empty;

        public DateTime? Date { get; set; } = DateTime.Now;
    }
}
