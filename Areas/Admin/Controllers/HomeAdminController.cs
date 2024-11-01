using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLBN.Models;
using QLBN.Models.Authentication;
using X.PagedList;
using X.PagedList.Mvc.Core;
using Microsoft.AspNetCore.WebUtilities;
using QLBN.ViewModels;

namespace QLBN.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    //[Route("admin/homeadmin")]
    
    //[Authorize(Roles ="admin")]
    public class HomeAdminController : Controller
    {
        QLBNContext db = new QLBNContext();
        
        public bool Access()
        {
            if (HttpContext.Session.GetString("Role") == "Admin")
                return true;
            else return false;
        }
        //[Route("")]
        [Route("index")] // co the se sua o day
        public IActionResult Index()
        {
            //if (HttpContext.Session.GetString("Role")=="Admin")
            //if(Access()) return View();
            //else return View("~/Views/Home/Index.cshtml");
            if (Access()==false) return View("~/Views/Home/Index.cshtml");
            return View();
        }
        [Route("danhmucbacsi")]
        public IActionResult DanhMucBacSi(int? page)
        {
            if (!Access()) return View("~/Views/Home/Index.cshtml");
           
            int pageSize = 8;
            //int pageNumber = pageSize == null || pageSize < 0 ? 1 : page.Value;
            int pageNumber = page ?? 1;
            var lstsanpham = db.Doctors.AsNoTracking().OrderBy(x => x.FacultyId);
            PagedList<Doctor> lst= new PagedList<Doctor>(lstsanpham,pageNumber,pageSize);

           // var lstBacSi= db.Doctors.ToList();
            return View(lst);
        }
        [Route("ThemBacSiMoi")]
        [HttpGet]
        public IActionResult ThemBacSiMoi()
        {
            if (!Access()) return View("~/Views/Home/Index.cshtml");
            ViewBag.FacultyId = new SelectList(db.Faculties.ToList(), "FacultyId", "FacultyName");
            ViewBag.RoomId = new SelectList(db.Rooms.ToList(), "RoomId", "RoomId" );
            ViewBag.ServiceId = new SelectList(db.Services.ToList(), "ServiceId", "ServiceName");

            return View();
        }
        [Route("ThemBacSiMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemBacSiMoi(Doctor doctor)
        {
            if (!Access()) return View("~/Views/Home/Index.cshtml");
            if (ModelState.IsValid)
            {
                db.Doctors.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("DanhMucBacSi");
            }
            return View(doctor);
        }
       [Route("SuaBacSi")]
        [HttpGet]
        public IActionResult SuaBacSi(string doctorId)
        {
            if (!Access()) return View("~/Views/Home/Index.cshtml");
            ViewBag.FacultyId = new SelectList(db.Faculties.ToList(), "FacultyId", "FacultyName");
            ViewBag.RoomId = new SelectList(db.Rooms.ToList(), "RoomId", "RoomId");
            ViewBag.ServiceId = new SelectList(db.Services.ToList(), "ServiceId", "ServiceName");
            var BacSi = db.Doctors.Find(Convert.ToInt32(doctorId));
            return View(BacSi);
        }
        [Route("SuaBacSi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaBacSi(Doctor doctor)
        {
            if (!Access()) return View("~/Views/Home/Index.cshtml");
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhMucBacSi","HomeAdmin");
            }
            return View(doctor);
        }
        [Route("XoaBacSi")]
        [HttpGet]
        public IActionResult XoaBacSi(string doctorId)
        {

            if (!Access()) return View("~/Views/Home/Index.cshtml");
            var lichhen = db.Appointments
            .Where(x => x.DoctorId == Convert.ToInt32(doctorId) && x.AppointmentDate > DateTime.Now)
            .ToList();
            if (lichhen.Count()>0)
            {
                TempData["Message"] = " Không thể xóa vì còn lịch hẹn";
                return RedirectToAction("DanhMucBacSi", "HomeAdmin");
            }
            db.Remove(db.Doctors.Find(Convert.ToInt32(doctorId)));
            db.SaveChanges();
            TempData["Message"] = "Bác sĩ đã được xóa";
            return RedirectToAction("DanhMucBacSi", "HomeAdmin");
        }
        [Route("DanhMucLichHen")]
        public IActionResult DanhMucLichHen(int? page)
        {
            // Logic kiểm tra quyền truy cập ở đây (ví dụ: sử dụng AuthorizationAttribute)
            // ...
            if (!Access()) return View("~/Views/Home/Index.cshtml");
            int pageSize = 8;
            int pageNumber = page ?? 1;

            // Get appointments ordered by date (assuming 'AppointmentDate' exists in Appointment model)
            var appointments = db.Appointments.AsNoTracking().OrderBy(x => x.AppointmentId);
            PagedList<Appointment> lst = new PagedList<Appointment>(appointments, pageNumber, pageSize);
            return View(lst);
        }

        [Route("ThemLichHenMoi")]
        [HttpGet]
        public IActionResult ThemLichHenMoi()
        {
            var faculties = db.Faculties.ToList();
            if (faculties == null || !faculties.Any())
            {
                ModelState.AddModelError("", "No faculties available.");
                return View(); // Hoặc hiển thị thông báo phù hợp
            }
            ViewBag.Faculties = new SelectList(faculties, "FacultyId", "FacultyName");
            ViewBag.Doctors = new SelectList(db.Doctors
            .Select(d => new
            {
                d.DoctorId,
                FullName = $"{d.DoctorDegree} - {d.DoctorName}"  // Nối chuỗi ở đây
            }).ToList(), "DoctorId", "FullName");

            ViewBag.Services = new SelectList(db.Services.ToList(), "ServiceId", "ServiceName");
            return View();
        }
        [Route("ThemLichHenMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemLichHenMoi(PatientAppointmentViewModel model)
        {
            if (!Access()) return View("~/Views/Home/Index.cshtml");

            // Kiểm tra nếu DoctorId tồn tại trong bảng Doctor
            if (ModelState.IsValid)
            {
                Console.WriteLine($"DoctorId: {model.DoctorId}"); // Debug line
                                                                  // Các đoạn mã khác...
                var patient = new Patient
                {
                    PatientName = model.PatientName,
                    PatientAddress = model.PatientAddress,
                    PatientBorn = model.PatientBorn,

                    PatientEmail = model.PatientEmail,
                    PatientGender = model.PatientGender,
                    PatientPhone = model.PatientPhone,
                    PatientId = model.PatientId,
                };
                if (db.Patients.Find(patient.PatientId) == null)
                {
                    try
                    {
                        db.Patients.Add(patient);
                        db.SaveChanges();
                    }
                    catch (DbUpdateException ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                    }
                }

                var doctor = db.Doctors.FirstOrDefault(x => x.DoctorId == model.DoctorId); // kiểm tra xem có doctor trả về không
                var faculty = db.Faculties.FirstOrDefault(x => x.FacultyId == model.FacultyId); // kiểm tra có kết quả trả về không
                if (doctor == null)
                {
                    if (faculty == null) ModelState.AddModelError("", "Khoa không tồn tại."); // không có khoa, bác sĩ trả về

                    ViewBag.Faculties = new SelectList(db.Faculties.ToList(), "FacultyId", "FacultyName");
                    ViewBag.Doctors = new SelectList(db.Doctors
                    .Select(d => new
                    {
                        d.DoctorId,
                        FullName = $"{d.DoctorDegree} - {d.DoctorName}"  // Nối chuỗi ở đây
                    }).ToList(), "DoctorId", "FullName");
                    ViewBag.Services = new SelectList(db.Services.ToList(), "ServiceId", "ServiceName");

                    ModelState.AddModelError("", "Bác sĩ không tồn tại.");
                    return View(model); // Trả về view với thông báo lỗi
                }
                int serviceId = Convert.ToInt32(doctor.ServiceId);
                var appoinment = new Appointment
                {
                    AppointmentDate = model.AppointmentDate,
                    AppointmentDescription = model.AppointmentDescription,
                    DoctorId = model.DoctorId,
                    PatientId = model.PatientId,
                    ServiceId = model.ServiceId,
                    AppointmentStatus = "Pending", // đang chờ


                };
                db.Appointments.Add(appoinment);
                try
                {
                    db.Appointments.Add(appoinment);
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                }
                return RedirectToAction("Index"); // sau khi create thì trả về index danh sách bác sĩ,
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }
            return View(model);
        }
       // [Route("GetDoctorsByFacultyAdmin")]
        public IActionResult GetDoctorsByFacultyAdmin(int facultyId, int serviceId)
        {
            var doctors = db.Doctors
                    .Where(d => d.FacultyId == facultyId && d.ServiceId == serviceId)
                    .Select(d => new
                    {
                        Id = d.DoctorId, // Đổi tên thành Id
                        //Name = d.DoctorName // Đổi tên thành Name
                        Name = $"{d.DoctorDegree} - {d.DoctorName}"
                        //FullName = $"{d.DoctorDegree} - {d.DoctorName}"
                    }).ToList();
            foreach (var doctor in doctors)
            {
                Console.WriteLine($"DoctorId: {doctor.Id}, FullName: {doctor.Name}");
            }


            return Json(doctors); // Trả về danh sách bác sĩ dưới dạng JSON
        }


        [Route("SuaLichHen")]
        [HttpGet]
        public IActionResult SuaLichHen(int appointmentId)
        {
            if (!Access())
            {
                return View("~/Views/Home/Index.cshtml");
            }
            if (appointmentId == 0 || db.Appointments == null)
            {
                return NotFound();
            }
            var LichHen = db.Appointments.Find(appointmentId);
            if (LichHen == null)
            {
                return NotFound();
            }

            ViewBag.ServiceId = new SelectList(db.Services.ToList(), "ServiceId", "ServiceName");
            return View(LichHen);
        }
        [Route("SuaLichHen")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaLichHen([Bind("AppointmentId, AppointmentDate, PatientId, DoctorId, ServiceId, AppointmentStatus")] Appointment appointment)
        {
            if (!Access())
            {
                Console.WriteLine("Access denied.");
                return View("~/Views/Home/Index.cshtml");
            }

            if (ModelState.IsValid == false)
            {

                try
                {


                    db.Entry(appointment).State = EntityState.Modified;
                    db.SaveChanges();


                    return RedirectToAction("DanhMucLichHen", "HomeAdmin");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.AppointmentId))
                    {
                        Console.WriteLine("Appointment does not exist.");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                Console.WriteLine("Model state is not valid.");
            }

            ViewBag.ServiceId = new SelectList(db.Services, "ServiceId", "ServiceName", appointment.ServiceId);
            return View(appointment);
        }


        // Phương thức kiểm tra sự tồn tại của một Appointment
        private bool AppointmentExists(int id)
        {
            return db.Appointments.Any(e => e.AppointmentId == id);
        }


        [Route("XoaLichHen")]
        [HttpGet]
        public IActionResult XoaLichHen(string appointmentId)
        {
            if (!Access()) return View("~/Views/Home/Index.cshtml");

            if (!int.TryParse(appointmentId, out var id))
            {
                TempData["Message"] = "ID lịch hẹn không hợp lệ.";
                return RedirectToAction("DanhMucLichHen", "HomeAdmin");
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var appointment = db.Appointments.Find(id);
                    var record = db.Records.Find(id);

                    if (appointment == null && record == null)
                    {
                        TempData["Message"] = "Lịch hẹn không tồn tại.";
                        return RedirectToAction("DanhMucLichHen", "HomeAdmin");
                    }

                    if (record != null)
                    {
                        db.Records.Remove(record);
                    }

                    if (appointment != null)
                    {
                        db.Appointments.Remove(appointment);
                    }

                    db.SaveChanges();
                    transaction.Commit();
                    TempData["Message"] = "Lịch hẹn đã được xóa.";
                    return RedirectToAction("DanhMucLichhen", "HomeAdmin");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["Message"] = "Có lỗi xảy ra khi xóa lịch hẹn.";
                    return RedirectToAction("DanhMucLichHen", "HomeAdmin");
                }
            }
        }
        [Route("ChiTietLichHen")]
        [HttpGet]
        public IActionResult ChiTietLichHen(int appointmentid)
        {

            if (appointmentid == null || db.Appointments == null)
            {
                return NotFound();
            }
            var appointment = db.Appointments.Find(appointmentid); // tìm appointment

            string patientId = appointment.PatientId; // tìm mã bệnh nhân từ PatientId

            var patient = db.Patients.FirstOrDefault(x => x.PatientId == patientId);


            int doctorId = appointment.DoctorId; // lấy ra mã bác sĩ từ lịch hẹn
            var doctor = db.Doctors.FirstOrDefault(y => y.DoctorId == doctorId);
            string doctorName = doctor.DoctorName;


            int serviceId = doctor?.ServiceId ?? 0; // Gán giá trị mặc định là 0 nếu ServiceId là null
            var service = db.Services.FirstOrDefault(z => z.ServiceId == serviceId);
            string serviceName = service.ServiceName;
            int serviceFee = service.ServiceFee;


            int facultyId = doctor?.FacultyId ?? 0; // Gán giá trị mặc định là 0 nếu FacultyId là null
            var faculty = db.Faculties.FirstOrDefault(t => t.FacultyId == facultyId);
            string facultyName = faculty.FacultyName;

            string roomId = doctor.RoomId;
            var room = db.Rooms.FirstOrDefault(m => m.RoomId == roomId);


            DetailAppointmentViewModel viewModel = new DetailAppointmentViewModel
            {
                PatientId = patient.PatientId,
                PatientName = patient.PatientName,
                PatientEmail = patient.PatientEmail,
                PatientPhone = patient.PatientPhone,
                PatientBorn = patient.PatientBorn,
                PatientGender = patient.PatientGender,
                PatientAddress = patient.PatientAddress,
                AppointmentDescription = appointment.AppointmentDescription,
                DoctorName = doctor.DoctorName,
                ServiceName = service.ServiceName,
                ServiceFee = service.ServiceFee,
                FacultyName = faculty.FacultyName,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentId = appointment.AppointmentId,
                RoomNumber = room.RoomNumber,
                RoomFloor = room.RoomFloor,
                RoomBuilding = room.RoomBuilding
            };

            return View(viewModel);
        }
    }
}
