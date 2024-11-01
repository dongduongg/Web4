using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using QLBN.Models;
using QLBN.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using QLBN.ViewModels;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Extensions;
//using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;
using QLBN.Helper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;

namespace QLBN.Controllers
{
    
    public class AccessController : Controller
    {
        QLBNContext db = new QLBNContext();
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccessController> _logger;

        private int OTP;
     

        //public AccessController( QLBNContext db, SignInManager<User> signInManager, UserManager<User> userManager, IEmailSender emailSender, ILogger<AccessController> logger)
        //{
        //    this.db = db;
        //    _signInManager = signInManager;
        //    _userManager = userManager;
        //    _emailSender = emailSender;
        //    _logger = logger;
        //}
        //public AccessController(QLBNContext db, SignInManager<User> signInManager)
        //{
        //    db = db;
        //    _signInManager = signInManager;
        //}

        public string us { get; set; }
        public string pwd { get; set; }
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") == null)
                return View();
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            string expectedRole = "Admin";
            if (HttpContext.Session.GetString("Username") == null)
            {
                var u = db.Users.Where(x => x.Username.Equals(user.Username) && x.Password.Equals(user.Password)).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("Username", u.Username.ToString()); // thiết lập một session có tên username được gán bởi tên người dùng, xảy ra khi nhập dữ liệu cho người dùng có username nằm trong cơ sở dữ liệu và cần đảo bảo nó chwua tồn tại ==null
                    HttpContext.Session.SetString("Role", "User");// set role là user
                    if (user.Username == "admin"|| user.Username == "Admin" || user.Role=="admin"||user.Role=="Admin")
                    //if(HttpContext.Session.GetString("Role").ToLower()==u.Role.ToString().ToLower())
                    {
                        HttpContext.Session.SetString("Role", expectedRole);
                        return View("~/Areas/Admin/Views/HomeAdmin/Index.cshtml");
                    }

                    else return RedirectToAction("Index", "Home");
                }
            }
            return View();



        }
        //[HttpGet]
        //public IActionResult LoginWithGoogle()
        //{
        //    var redirectUrl = Url.Action("ExternalLogin", "Access");
        //    var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        //    return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        //}

        //public async Task<IActionResult> ExternalLogin(string provider, string returnUrl = "/")
        //{
        //    var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    if (!result.Succeeded) return RedirectToAction("Login");

        //    var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
        //    var email = claims?.FirstOrDefault(c => c.Type == "email")?.Value;

        //    if (email != null)
        //    {
        //        var user = db.Users.FirstOrDefault(u => u.Username == email);
        //        if (user == null)
        //        {
        //            // Tạo người dùng mới nếu chưa tồn tại
        //            user = new User { Username = email, Role = "User" };
        //            db.Users.Add(user);
        //            db.SaveChanges();
        //        }

        //        HttpContext.Session.SetString("Username", user.Username);
        //        HttpContext.Session.SetString("Role", user.Role);

        //        return RedirectToAction("Index", "Home");
        //    }

        //    return RedirectToAction("Login");
        //}
        [AllowAnonymous]
        public IActionResult SignUp() 
        {

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //public IActionResult SignUp(PatientUserViewModel patientUser)
        public async Task<IActionResult> SignUp(PatientUserViewModel patientUser)
        {
            
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(patientUser);
            }
            else 
            {
                

                var patientId =patientUser.PatientId;
                var check = db.Patients.FirstOrDefault(x=>x.PatientId.Equals(patientId));
                if (check != null)
                {
                    ModelState.AddModelError("PatientId", "Mã bệnh nhân đã tồn tại.");
                    return View(patientUser); // Trả về view với lỗi
                }
                
                    
                var user = new User
               // user = new User
                {
                    Username = patientUser.Username,
                    Password = patientUser.Password,
                    Role = "user"

                };
                var patient = new Patient
                //patient = new Patient
                {
                    PatientId = patientUser.PatientId,
                    PatientAddress = patientUser.PatientAddress,
                    PatientBorn = patientUser.PatientBorn,
                   
                    PatientName = patientUser.PatientName,
                    PatientPhone = patientUser.PatientPhone,
                    PatientEmail = patientUser.PatientEmail,
                    PatientGender = patientUser.PatientGender,
                    Username = patientUser.Username
                };

              

                //try
                //{
                //    db.Users.Add(user);
                //    db.Patients.Add(patient);
                //    db.SaveChanges();
                //}
                //catch (DbUpdateException ex)
                //{
                //    Console.WriteLine($"Error: {ex.Message}");
                //    Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                //}

                HttpContext.Session.SetString("Username", patientUser.Username);
                HttpContext.Session.SetString("Password", patientUser.Password);
                HttpContext.Session.SetString("PatientId", patientUser.PatientId);
                HttpContext.Session.SetString("PatientAddress", patientUser.PatientAddress);
                HttpContext.Session.SetString("PatientBorn", Convert.ToString(patientUser.PatientBorn));
                HttpContext.Session.SetString("PatientName", patientUser.PatientName);
                HttpContext.Session.SetString("PatientPhone", patientUser.PatientPhone);
                HttpContext.Session.SetString("PatientEmail", patientUser.PatientEmail);
                HttpContext.Session.SetString("PatientGender", patientUser.PatientGender);
 
                Random random = new Random();
                OTP = random.Next(100000, 1000000);
                HttpContext.Session.SetInt32("OTP", OTP);
                string code = patientUser.Password;
                var callbackUrl = Url.Action("ConfirmEmail", "Access", new { userName = patientUser.Username, code = code }, protocol: Request.Scheme);
                //string body = $"Please confirm your email account is true by clicking <a href=\"{callbackUrl}\">here</a>";
                string body = $"Please confirm your email account is true by OPT number  "+OTP;
                SendMail.SendEmail(patientUser.PatientEmail, "Confirm your account", body, "");

                // Chuyển hướng đến trang thông báo đã gửi email xác nhận
                return View("EmailConfirmation");

                // return RedirectToAction("Login");


            }
           
            
           
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult EmailConfirmation()
        {
            return View();
        }
        [HttpPost]
        public IActionResult EmailConfirmation(string OTP)
        {
            var storedOtp = HttpContext.Session.GetInt32("OTP");

            if (storedOtp.HasValue && OTP == storedOtp.ToString())
            {
                var username = HttpContext.Session.GetString("Username");
                var password = HttpContext.Session.GetString("Password");
                var patientId = HttpContext.Session.GetString("PatientId");
                var patientAddress = HttpContext.Session.GetString("PatientAddress");
                var patientBorn = HttpContext.Session.GetString("PatientBorn");
                var patientName = HttpContext.Session.GetString("PatientName");
                var patientPhone = HttpContext.Session.GetString("PatientPhone");
                var patientEmail = HttpContext.Session.GetString("PatientEmail");
                var patientGender = HttpContext.Session.GetString("PatientGender");

                // Tạo đối tượng user và patient
                var user = new User
                {
                    Username = username,
                    Password = password,
                    Role = "user"
                };

                var patient = new Patient
                {
                    PatientId = patientId,
                    PatientAddress = patientAddress,
                    PatientBorn = Convert.ToDateTime(patientBorn),
                    PatientName = patientName,
                    PatientPhone = patientPhone,
                    PatientEmail = patientEmail,
                    PatientGender = patientGender,
                    Username = username
                };
                try
                {
                    db.Users.Add(user);
                    db.Patients.Add(patient);
                    db.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                }

                HttpContext.Session.SetString("Username", user.Username); // thiết lập một session có tên username được gán bởi tên người dùng, xảy ra khi nhập dữ liệu cho người dùng có username nằm trong cơ sở dữ liệu và cần đảo bảo nó chwua tồn tại ==null
                HttpContext.Session.SetString("Role", "User");// set role là user
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Mã OTP không chính xác.");
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userName, string code)
        {
            if (userName == null || code == null)
            {
                return RedirectToAction("Index", "Home"); // Hoặc trang lỗi
            }

            var user =db.Users.Where( x=>x.Username == userName);
            if (user == null)
            {
                // Hoặc trang lỗi
                // Xác nhận thất bại
                return View("Privacy","Home");
            }

            //var result = await _userManager.ConfirmEmailAsync(user, code);
            //if (result.Succeeded)
            //{
            //    // Xác nhận thành công, có thể chuyển hướng đến trang đăng nhập hoặc trang thành công
            //    return View("ConfirmEmail");
            //}
            else
            {
                HttpContext.Session.SetString("Username", userName); // thiết lập một session có tên username được gán bởi tên người dùng, xảy ra khi nhập dữ liệu cho người dùng có username nằm trong cơ sở dữ liệu và cần đảo bảo nó chwua tồn tại ==null
                HttpContext.Session.SetString("Role", "User");// set role là user
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult VerifyPhone()
        {
            ViewBag.PhoneNumber = TempData["PhoneNumber"];
            return View();
        }

        [HttpPost]
        public IActionResult VerifyPhone(string otp)
        {
            var expectedOtp = TempData["OTP"]?.ToString();
            var patientUser = TempData["PatientInfo"] as PatientUserViewModel;
            if (otp == expectedOtp && patientUser != null)
            {
                // Xác thực thành công, cập nhật trạng thái của người dùng
                var user = new User
                {
                    Username = patientUser.Username,
                    Password = patientUser.Password,
                    Role = "user"

                };
                db.Users.Add(user);
                db.SaveChanges();
                var patient = new Patient
                {
                    PatientId = patientUser.PatientId,
                    PatientAddress = patientUser.PatientAddress,
                    PatientBorn = patientUser.PatientBorn,
                    
                    PatientName = patientUser.PatientName,
                    PatientPhone = patientUser.PatientPhone,
                    PatientEmail = patientUser.PatientEmail,
                    PatientGender = patientUser.PatientGender,

                };
                db.Patients.Add(patient);
                db.SaveChanges();


                return RedirectToAction("Login");
            }

            ViewBag.Error = "Mã OTP không chính xác.";
            return View();
        }
 
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Username");
            return RedirectToAction("Login", "Access");
        }
       



    }
}
