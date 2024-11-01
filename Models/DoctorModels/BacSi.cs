namespace QLBN.Models.DoctorModels
{
    public class BacSi
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = null!;
        public string ImagePath { get; set; }
        public string? DoctorDegree { get; set; }
        public int? FacultyId { get; set; }
        public int? ServiceId { get; set; }
        public string? FacultyName { get; set; } // co the xoa bo o day
    }
}
