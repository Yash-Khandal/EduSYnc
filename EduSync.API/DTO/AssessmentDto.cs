public class AssessmentDto
{
    public Guid AssessmentId { get; set; }
    public Guid CourseId { get; set; }
    public string Title { get; set; }
    public string Questions { get; set; }
    public int MaxScore { get; set; }
    public Guid InstructorId { get; set; }         // <-- Add this
    public string InstructorName { get; set; }     // <-- Optional, for display
}
