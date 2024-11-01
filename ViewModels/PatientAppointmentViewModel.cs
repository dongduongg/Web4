using QLBN.Models;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace QLBN.ViewModels
{
    public class PatientAppointmentViewModel
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
        [Required(ErrorMessage = "Bác sĩ là bắt buộc.")]
        public int DoctorId { get; set; }  //Khóa ngoại liên kết bảng bác sĩ
        [Required(ErrorMessage = "Dịch vụ là bắt buộc.")]
        public int ServiceId { get; set; } // khóa ngoại liên kết bảng service
        [Required(ErrorMessage = "Khoa là bắt buộc.")]
        public int FacultyId { get; set; } // khoá ngoại liên kết khoa

        [Required(ErrorMessage = "Giờ hẹn là bắt buộc.")]
        public DateTime AppointmentDate { get; set; }  // Thời gian hẹn

        
    }
}
