using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Models
{
    public class GigFormViewModel
    {
        [Required]
        public string Venue { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public string Time { get; set; }
        [Required]
        public byte Genre { get; set; }

        public List<Genre> Genres { get; set; }

        public DateTime DateTime => DateTime.Parse($"{Date}{Time}");
    }
}