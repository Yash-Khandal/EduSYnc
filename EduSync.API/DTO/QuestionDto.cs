using System.Collections.Generic;

namespace EduSync.DTOs
{
    public class QuestionDto
    {
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public int Marks { get; set; }
    }
}
