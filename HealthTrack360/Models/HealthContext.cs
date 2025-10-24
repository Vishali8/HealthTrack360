using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace HealthTrack360.Models
{
    public class HealthContext :DbContext
    {
        public DbSet<Recruiter> Recruiters { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Vital> Vitals { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public HealthContext() : base() { }
    }
}