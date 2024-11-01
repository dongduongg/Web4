using System.ComponentModel.DataAnnotations;

namespace QLBN.ViewModels
{
    public class PatientUserViewModel
    {
        
        public string PatientId { get; set; } = null!;
        public string PatientName { get; set; } = null!;
        [Required(ErrorMessage = "Email là bắt buộc")]
        public string? PatientEmail { get; set; }
        public string? PatientPhone { get; set; }
        public DateTime PatientBorn { get; set; }
        public string PatientGender { get; set; } = null!;
        public string PatientAddress { get; set; } = null!;
        public string? PatientDesciption { get; set; }
        [Required(ErrorMessage = "Username là bắt buộc")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập mật khẩu")] 
        public string Password { get; set; }
        public string Role { get; set; } = "user";
    }
}
