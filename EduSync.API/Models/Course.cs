using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduSync.Models
{
    public class Course
    {
        [Key]
        public Guid CourseId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public Guid InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public virtual User Instructor { get; set; }

        // URL to media file (video, pdf, csv, etc.)
        public string MediaUrl { get; set; }
    }
}
