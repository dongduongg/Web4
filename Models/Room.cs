using System;
using System.Collections.Generic;

namespace QLBN.Models
{
    public partial class Room
    {
        public Room()
        {
            Doctors = new HashSet<Doctor>();
        }

        public string RoomId { get; set; } = null!;
        public int RoomNumber { get; set; }
        public string? RoomFloor { get; set; }
        public string? RoomBuilding { get; set; }
        public int FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; } = null!;
        public virtual ICollection<Doctor> Doctors { get; set; }
    }
}
