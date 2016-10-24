using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Models
{
    /// <summary>
    /// 现场演唱会
    /// </summary>
    public class Gig
    {
        public int Id { get; set; }
        [Required]
        public ApplicationUser Artist { get; set; }

        [Required]
        public string ArtistId { get; set; }
        public DateTime DateTime { get; set; }
        [Required]
        [StringLength(255)]
        public string Venue { get; set; }
        [Required]
        public Genre Genre { get; set; }

        [Required]
        public byte GenreId { get; set; }
    }
}