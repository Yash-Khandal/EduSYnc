using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduSync.Data;
using EduSync.Models;
using EduSync.DTOs;
using EduSync.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduSync.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly EduSyncDbContext _context;
        private readonly BlobStorageService _blobService;

        public CoursesController(EduSyncDbContext context, BlobStorageService blobService)
        {
            _context = context;
            _blobService = blobService;
        }

        // POST: api/courses
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromForm] CourseForCreationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var instructor = await _context.Users.FirstOrDefaultAsync(u =>
                u.UserId == dto.InstructorId && u.Role == "Instructor");

            if (instructor == null)
                return BadRequest("Invalid Instructor ID or user is not an Instructor.");

            string mediaUrl = null;
            if (dto.MediaFile != null)
            {
                mediaUrl = await _blobService.UploadFileAsync(dto.MediaFile);
            }

            var course = new Course
            {
                CourseId = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                InstructorId = dto.InstructorId,
                MediaUrl = mediaUrl
            };

            _context.Courses.Add(course);
            await _context.SaveChangesAsync();

            // Return CourseDto (optional)
            return CreatedAtAction(nameof(GetCourseById), new { id = course.CourseId }, new
            {
                course.CourseId,
                course.Title,
                course.Description,
                course.InstructorId,
                InstructorName = instructor.Name,
                course.MediaUrl
            });
        }

        // GET: api/courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.Instructor)
                .Select(c => new
                {
                    c.CourseId,
                    c.Title,
                    c.Description,
                    c.InstructorId,
                    InstructorName = c.Instructor.Name,
                    c.MediaUrl
                })
                .ToListAsync();

            return Ok(courses);
        }

        // GET: api/courses/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourseById(Guid id)
        {
            var course = await _context.Courses
                .Include(c => c.Instructor)
                .Where(c => c.CourseId == id)
                .Select(c => new
                {
                    c.CourseId,
                    c.Title,
                    c.Description,
                    c.InstructorId,
                    InstructorName = c.Instructor.Name,
                    c.MediaUrl
                })
                .FirstOrDefaultAsync();

            if (course == null)
                return NotFound();

            return Ok(course);
        }

        // PUT: api/courses/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromForm] CourseForCreationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound($"Course with ID {id} not found.");

            // Optional: Validate instructor
            if (course.InstructorId != dto.InstructorId)
            {
                var instructor = await _context.Users.FirstOrDefaultAsync(u => u.UserId == dto.InstructorId && u.Role == "Instructor");
                if (instructor == null)
                    return BadRequest("Invalid new Instructor ID or user is not an Instructor.");
            }

            course.Title = dto.Title;
            course.Description = dto.Description;
            course.InstructorId = dto.InstructorId;

            if (dto.MediaFile != null)
            {
                course.MediaUrl = await _blobService.UploadFileAsync(dto.MediaFile);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/courses/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound($"Course with ID {id} not found.");

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
