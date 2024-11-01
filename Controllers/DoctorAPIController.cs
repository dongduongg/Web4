using Microsoft.AspNetCore.Mvc;
using QLBN.Models;
using Microsoft.AspNetCore.Http;
using QLBN.Models.DoctorModels;
//using QLBN.Models.Authentication;

namespace QLBN.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    
    public class DoctorAPIController : ControllerBase

    {
        QLBNContext db= new QLBNContext();
        [HttpGet]

        public IEnumerable<BacSi> GetAllDoctors()
        {
            var bacsi= (from p in db.Doctors select new BacSi
            {
                DoctorId = p.DoctorId,
                DoctorName=p.DoctorName,
                FacultyId = p.FacultyId,
                ServiceId= p.ServiceId,
                ImagePath = p.ImagePath,
            }).ToList();
            return bacsi;
        }
        [HttpGet("{maKhoa}")]
        public IEnumerable<BacSi> GetDoctorsByFaculty(string maKhoa) // https://localhost:7163/api/doctorapi/97 sẽ đưa ra các bác sĩ chỉ thuộc khoa 97
        {
            var bacsi = (from p in db.Doctors join f in db.Faculties on p.FacultyId equals f.FacultyId
                         where p.FacultyId == Convert.ToInt32(maKhoa)
                         select new BacSi
                         {   ImagePath= p .ImagePath,
                             DoctorId = p.DoctorId,
                             DoctorName = p.DoctorName,
                             DoctorDegree=p.DoctorDegree,
                             FacultyId = f.FacultyId,
                             ServiceId = p.ServiceId,
                             FacultyName= f.FacultyName,
                         }).ToList();
            return bacsi;
        }
    }
}
