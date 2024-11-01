using System;
using System.Collections.Generic;

namespace QLBN.Models
{
    public partial class Record
    {
        public int RecordId { get; set; }
        public string? RecordDiagnosis { get; set; }
        public int AppointmentId { get; set; }

        public virtual Appointment Appointment { get; set; } = null!;
    }
}
