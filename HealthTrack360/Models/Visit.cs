
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
namespace HealthTrack360.Models
{
    public class Visit
    {
        public int VisitID { get; set; }

        //foreign key reference to patient
        public int PatientID { get; set; }
        public DateTime VisitDate { get; set; }
        public string Department { get; set; }
        public string DoctorName { get; set; }
        public string Reason { get; set; }

        [ForeignKey("PatientID")]
        public virtual Patient Patient { get; set; }
        public virtual ICollection<Vital> Vitals { get; set; }
    }
}