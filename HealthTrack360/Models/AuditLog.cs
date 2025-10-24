using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthTrack360.Models
{
    public class AuditLog
    {
        public int LogID { get; set; }
        public string TableName { get; set; }
        public string ActionType { get; set; }
        public int RecordId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}