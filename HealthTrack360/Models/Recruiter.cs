using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthTrack360.Models
{
    public class Recruiter
    {
        
        public int RecruiterId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Store hashed in production
    }
}