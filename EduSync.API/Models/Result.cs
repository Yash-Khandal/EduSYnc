using System;
using EduSync.Models;

namespace EduSync.Models
{
    public class Result
    {
        public Guid ResultId { get; set; }

        public Guid AssessmentId { get; set; }
        public Assessment Assessment { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; } // ✅ Add this to fix "User.Name" error

        public int Score { get; set; }

        public DateTime AttemptDate { get; set; }
    }
}
