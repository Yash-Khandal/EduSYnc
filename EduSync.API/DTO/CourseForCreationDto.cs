using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace EduSync.DTOs
{
    public class CourseForCreationDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title can't be longer than 200 characters.")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description can't be longer than 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Instructor ID is required.")]
        public Guid InstructorId { get; set; }

        // For file upload (PDF, CSV, etc.)
        public IFormFile MediaFile { get; set; }

        // Optional: MediaUrl string if file is already uploaded or external link
        public string MediaUrl { get; set; }
    }
}
