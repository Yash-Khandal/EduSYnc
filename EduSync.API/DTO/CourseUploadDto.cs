using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace EduSync.API.DTOs
{
    public class CourseUploadDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Guid InstructorId { get; set; }

        [Required]
        public IFormFile PdfFile { get; set; }
    }
}
