using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthTrack360.Models
{
    public class PatientDetails
    {
        public Patient Patient { get; set; }
        public List<Visit> Visits { get; set; }
        public List<Vital> Vitals { get; set; }
    }
}