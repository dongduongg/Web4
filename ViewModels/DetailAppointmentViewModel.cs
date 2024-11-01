using QLBN.Models;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace QLBN.ViewModels
{
    public class DetailAppointmentViewModel
    {
        public int AppointmentId { get; set; } // Thêm thuộc tính này
        public string PatientId { get; set; } = null!;
        public string PatientName { get; set; } = null!;
        public string? PatientEmail { get; set; }
        public string? PatientPhone { get; set; }
        public DateTime PatientBorn { get; set; }
        public string PatientGender { get; set; } = null!;
        public string PatientAddress { get; set; } = null!;
        public string? AppointmentDescription { get; set; }
        //public int DoctorId { get; set; }  
        public string DoctorName { get; set; }
       // public int ServiceId { get; set; } 
        public string ServiceName { get; set; } 
        public int ServiceFee { get; set; }
       
       // public int FacultyId { get; set; } 
        public string FacultyName { get; set; }
        public int RoomNumber { get; set; }
        public string RoomFloor { get; set; }
        public string RoomBuilding { get; set; }

        
        public DateTime AppointmentDate { get; set; }  // Thời gian hẹn
    }
}
