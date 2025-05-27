using System;
using System.Collections.Generic;

namespace EduSync.DTOs
{
    public class StudentSubmissionDto
    {
        public Guid AssessmentId { get; set; }
        public Guid StudentId { get; set; }
        public List<AnswerSubmissionDto> Answers { get; set; }
    }

    public class AnswerSubmissionDto
    {
        public string QuestionText { get; set; }
        public string SelectedOption { get; set; }
    }
}
