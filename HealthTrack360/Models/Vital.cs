using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
namespace HealthTrack360.Models
{
    public class Vital
    {
        public int VitalID { get; set; }

        //foreign key reference to Visit
        public int VisitID { get; set; }
        public string BloodPressure { get; set; }
        public decimal SugarLevel { get; set; }
        public int PulseRate { get; set; }
        public DateTime RecordedAt { get; set; }

        [ForeignKey("VisitID")]
        public virtual Visit Visit { get; set; }

        public virtual ICollection<Alert> Alerts { get; set; }
    }
}