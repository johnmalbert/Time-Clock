using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Time_Clock.Models
{
    public class TimeStamp
    {
        [Key]
        public int TimeStampId { get; set; }
        public string Job { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        [NotMapped]
        public string ID { get; set; }
        [NotMapped]
        [MinLength(4, ErrorMessage="Pin must be four digits long")]
        public string Pin { get; set; }
        [NotMapped]
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}