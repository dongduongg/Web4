using System;
using System.Collections.Generic;

namespace QLBN.Models
{
    public partial class Doctor
    {
        public Doctor()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = null!;
        public string? DoctorDegree { get; set; }
        public int? FacultyId { get; set; }
        public string? RoomId { get; set; }
        public int? ServiceId { get; set; }
        public string? ImagePath { get; set; }
        public virtual Faculty? Faculty { get; set; }
        public virtual Room? Room { get; set; }
        public virtual Service? Service { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
