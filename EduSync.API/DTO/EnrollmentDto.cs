

// DTOs/EnrollmentDto.cs
using System;

namespace EduSync.DTOs
{
    public class EnrollmentDto
    {
        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public Guid CourseId { get; set; }
        public DateTime EnrolledAt { get; set; }
    }
}
