using System;
using System.Collections.Generic;

namespace QLBN.Models
{
    public partial class Service
    {
        public Service()
        {
            Appointments = new HashSet<Appointment>();
            Doctors = new HashSet<Doctor>();
        }

        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public int ServiceFee { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
