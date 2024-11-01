//using QLBN.Data;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLBN.Models;
using X.PagedList;
using X.PagedList.Mvc.Core;
using QLBN.ViewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using QLBN.Models.Authentication;
using System.Numerics;
using System.ComponentModel.DataAnnotations;
using MimeKit.Cryptography;

namespace QLBN.Controllers
{

    public class AppointmentController : Controller
    {

        QLBNContext db = new QLBNContext();
        public IActionResult Index()
        {
            string user = HttpContext.Session.GetString("Username");
            List<Appointment> listAppointment = new List<Appointment>();

            
            if (user != null)
            {
                var patient = db.Patients.FirstOrDefault(x => x.Username == user);
                try
                {
                    listAppointment = db.Appointments.Where(x => x.PatientId == patient.PatientId).ToList();
                    Console.WriteLine("List appointment >0");
                }
                catch(Exception ex) {
                    
                    Console.Write(ex.ToString());
                    return RedirectToAction("Create");
                }
                    
                
            }
            
            


            //var lst = db.Appointments.ToList();
            //return View(lst);
            return View(listAppointment);
        }

        [HttpGet]
        public IActionResult Create()
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create( PatientAppointmentViewModel model) //[Bind("PatientName,PatientId,PatientPhone,PatientEmail,PatientGender,PatientAddress,PatientBorn,AppointmentDate,FacultyID,DoctorID,ServiceId")]
        {
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
                    if(faculty == null) ModelState.AddModelError("", "Khoa không tồn tại."); // không có khoa, bác sĩ trả về

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
                int newAppointmentId = db.Appointments.Max(a => a.AppointmentId) + 1;
                var appoinment = new Appointment
                {
                    AppointmentId = newAppointmentId,
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
        [HttpGet]
        
        public IActionResult GetDoctorsByFaculty(int facultyId, int serviceId)
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
        public IActionResult Delete (int appointmentid)
        {
            if (appointmentid == null || db.Appointments == null)
            {
                return NotFound();
            }
            var appointment = db.Appointments
            .Where(x => x.AppointmentId == appointmentid && x.AppointmentDate > DateTime.Now)
            .FirstOrDefault();
            if (appointment == null)
            {
                return NotFound();
            }
            try
            {
                db.Appointments.Remove(appointment);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Xóa appointment"+ ex.Message);
            }
            return RedirectToAction("Index","Appointment");
        }
        [HttpGet]
        public IActionResult Edit(int appointmentid)
        {
            if (appointmentid == null || db.Appointments == null)
            {
                return NotFound();
            }
            var appointment = db.Appointments.Find(appointmentid);

            var faculties = db.Faculties.ToList();
            ViewBag.Faculties = new SelectList(faculties, "FacultyId", "FacultyName");
            ViewBag.Doctors = new SelectList(db.Doctors
            .Select(d => new
            {
                d.DoctorId,
                FullName = $"{d.DoctorDegree} - {d.DoctorName}"  // Nối chuỗi ở đây
            }).ToList(), "DoctorId", "FullName");
            ViewBag.Services = new SelectList(db.Services.ToList(), "ServiceId", "ServiceName");


            string patientId = appointment.PatientId; // tìm mã bệnh nhân từ PatientId
            var patient = db.Patients.FirstOrDefault(x => x.PatientId == patientId);
            int doctorId = appointment.DoctorId; // lấy ra mã bác sĩ từ lịch hẹn
            var doctor = db.Doctors.FirstOrDefault( y => y.DoctorId == doctorId);
            int serviceId = doctor?.ServiceId ?? 0; // Gán giá trị mặc định là 0 nếu ServiceId là null
            int facultyId = doctor?.FacultyId ?? 0; // Gán giá trị mặc định là 0 nếu FacultyId là null


            PatientAppointmentViewModel viewModel = new PatientAppointmentViewModel
            {
                PatientId = patient.PatientId,
                PatientName = patient.PatientName,
                PatientEmail = patient.PatientEmail,
                PatientPhone = patient.PatientPhone,
                PatientBorn = patient.PatientBorn,
                PatientGender = patient.PatientGender,
                PatientAddress = patient.PatientAddress,
                AppointmentDescription = appointment.AppointmentDescription,
                DoctorId = doctorId,
                ServiceId = serviceId,
                FacultyId = facultyId,
                AppointmentDate = appointment.AppointmentDate,
                AppointmentId = appointment.AppointmentId,
            };

            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(PatientAppointmentViewModel editmodel)
        {
            int appointmentid = editmodel.AppointmentId;
            if (ModelState.IsValid)
            {
                Console.WriteLine("MOdel edit isvalid");
               
                var doctor = db.Doctors.FirstOrDefault(x => x.DoctorId == editmodel.DoctorId); // kiểm tra xem có doctor trả về không
                var faculty = db.Faculties.FirstOrDefault(x => x.FacultyId == editmodel.FacultyId); // kiểm tra có kết quả trả về không
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
                    return View(editmodel); // Trả về view với thông báo lỗi
                }
                int serviceId = Convert.ToInt32(doctor.ServiceId);
                var appointment = db.Appointments.Find(appointmentid);
               
                try
                {
                    appointment.AppointmentDate = editmodel.AppointmentDate;
                    appointment.DoctorId = editmodel.DoctorId;
                    appointment.PatientId = editmodel.PatientId;
                    appointment.ServiceId = editmodel.ServiceId;
                    appointment.AppointmentStatus = "Pending"; // đang chờ
                    appointment.AppointmentId= editmodel.AppointmentId;

                    // Lưu thay đổi vào cơ sở dữ liệu

                    db.Entry(appointment).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                Console.WriteLine("MOdel edit not  valid");

            }
                return View(editmodel);
        }
        public IActionResult Details(int appointmentid)
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
            var service = db.Services.FirstOrDefault ( z=> z.ServiceId == serviceId);
            string serviceName = service.ServiceName;
            int serviceFee = service.ServiceFee;


            int facultyId = doctor?.FacultyId ?? 0; // Gán giá trị mặc định là 0 nếu FacultyId là null
            var faculty= db.Faculties.FirstOrDefault(t => t.FacultyId == facultyId);
            string facultyName = faculty.FacultyName;

            string roomId = doctor.RoomId;
            var room=db.Rooms.FirstOrDefault(m =>m.RoomId == roomId);


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
