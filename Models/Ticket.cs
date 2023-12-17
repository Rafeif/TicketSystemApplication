using System.ComponentModel.DataAnnotations;

namespace TicketSystemApplication.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Type { get; set; } = string.Empty;
        public string? UserId { get; set; } = string.Empty;


     
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;

        [Display(Name ="Ticket Status")]
        public int? TicketStatusId { get; set; } = 1;
        public  TicketStatus? TicketStatus { get; set; }
        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }
        public  Department? Department { get; set; }


    }

   
}
