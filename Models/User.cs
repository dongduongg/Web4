
using System;
using System.Collections.Generic;

namespace QLBN.Models
{
    public partial class User 
    {
        public User()
        {
            Patients = new HashSet<Patient>();
        }

        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;

        public virtual ICollection<Patient> Patients { get; set; }
    }
}
