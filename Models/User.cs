using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Time_Clock.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage="Name must be at least two characters")]
        public string Name { get; set; }

        [Required]
        [Range(1000,10000, ErrorMessage="Pin must be 4 digits")]
        public string Pin { get; set; }

        [NotMapped]
        [Compare("Pin", ErrorMessage="Confirm Pin must match Pin")]
        public string ConfirmPin { get; set; }

        public List<TimeStamp> TimeStamps { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}