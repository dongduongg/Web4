using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace QLBN.Models
{
    public partial class Appointment
    {
        public Appointment()
        {
            Records = new HashSet<Record>();
        }
       
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentStatus { get; set; } = null!;
        public string? AppointmentDescription { get; set; } 
        public int DoctorId { get; set; }
        public string PatientId { get; set; } = null!;
        public int ServiceId { get; set; }

        public virtual Doctor Doctor { get; set; } = null!;
        public virtual Patient Patient { get; set; } = null!;
        public virtual Service Service { get; set; } = null!;
        public virtual ICollection<Record> Records { get; set; }
    }
}
