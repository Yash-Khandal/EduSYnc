// Controllers/EnrollmentsController.cs
using EduSync.Data;
using EduSync.DTOs;
using EduSync.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EduSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private readonly EduSyncDbContext _context;

        public EnrollmentsController(EduSyncDbContext context)
        {
            _context = context;
        }

        // POST: api/enrollments/{courseId}/enroll
        [HttpPost("{courseId}/enroll")]
        public async Task<IActionResult> EnrollStudent(Guid courseId, [FromBody] EnrollRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = await _context.Courses.FindAsync(courseId);
            var student = await _context.Users.FirstOrDefaultAsync(u => u.UserId == dto.StudentId && u.Role == "Student");
            if (course == null || student == null)
                return BadRequest("Invalid course or student.");

            // Check if already enrolled
            var alreadyEnrolled = await _context.Enrollments.AnyAsync(e => e.CourseId == courseId && e.StudentId == dto.StudentId);
            if (alreadyEnrolled)
                return BadRequest("Student already enrolled.");

            var enrollment = new Enrollment
            {
                StudentId = dto.StudentId,
                CourseId = courseId
            };
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return Ok(new EnrollmentDto
            {
                EnrollmentId = enrollment.EnrollmentId,
                StudentId = enrollment.StudentId,
                CourseId = enrollment.CourseId,
                EnrolledAt = enrollment.EnrolledAt
            });
        }

        // GET: api/enrollments/student/{studentId}/courses
        [HttpGet("student/{studentId}/courses")]
        public async Task<IActionResult> GetStudentCourses(Guid studentId)
        {
            var courses = await _context.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                .Select(e => new
                {
                    e.Course.CourseId,
                    e.Course.Title,
                    e.Course.Description,
                    e.Course.InstructorId,
                    e.Course.MediaUrl
                })
                .ToListAsync();

            return Ok(courses);
        }

        // GET: api/enrollments/course/{courseId}/students
        [HttpGet("course/{courseId}/students")]
        public async Task<IActionResult> GetCourseStudents(Guid courseId)
        {
            var students = await _context.Enrollments
                .Where(e => e.CourseId == courseId)
                .Include(e => e.Student)
                .Select(e => new
                {
                    e.Student.UserId,
                    e.Student.Name,
                    e.Student.Email
                })
                .ToListAsync();

            return Ok(students);
        }
    }
}
