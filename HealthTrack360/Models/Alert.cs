using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthTrack360.Models
{
    public class Alert
    {
        
        public int AlertID { get; set; }
        public int VitalID { get; set; }
        public string AlertType { get; set; }
        public string Description { get; set; }
        public DateTime TriggeredAt { get; set; }

        [ForeignKey("VitalID")]
        public virtual Vital Vitals { get; set; }
    }
}