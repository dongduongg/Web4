using System;
using System.Collections.Generic;

namespace QLBN.Models
{
    public partial class Faculty
    {
        public Faculty()
        {
            Doctors = new HashSet<Doctor>();
            Rooms = new HashSet<Room>();
        }

        public int FacultyId { get; set; }
        public string FacultyName { get; set; } = null!;

        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
