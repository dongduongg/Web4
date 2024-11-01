using System;
using System.Collections.Generic;

namespace QLBN.Models
{
    public partial class Patient
    {
        public Patient()
        {
            Appointments = new HashSet<Appointment>();
        }

        public string PatientId { get; set; } = null!;
        public string PatientName { get; set; } = null!;
        public string? PatientEmail { get; set; }
        public string? PatientPhone { get; set; }
        public DateTime PatientBorn { get; set; }
        public string PatientGender { get; set; } = null!;
        public string PatientAddress { get; set; } = null!;
       
        public string Username { get; set; } = null!;

        public virtual User UsernameNavigation { get; set; } = null!;
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
