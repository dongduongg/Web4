using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using QLBN.Models;
using QLBN.ViewModels;
using BCrypt.Net;
using System.Text;

namespace QLBN.Controllers
{
    public class Profile : Controller
    {
        QLBNContext db = new QLBNContext();
        [HttpGet]
        public IActionResult SaveChanges()
        {
            string user = HttpContext.Session.GetString("Username");
           
            if (user != null)
            {
                ViewBag.User = db.Users.FirstOrDefault( x => x.Username == user);
                ViewBag.Patient = db.Patients.FirstOrDefault( x => x.Username.Equals(user) );
            }

            return View();
        }

        [HttpPost]
        public IActionResult SaveChanges(PatientUserViewModel patientUser) 
        { 
            return View();
        }
        [HttpGet]
        public IActionResult ChangePassword()
        { 
            return View();
        }
        [HttpPost]
        public IActionResult ChangePassword(UserChange userChange)
        {
            string Sessionname = HttpContext.Session.GetString("Username");
            Console.WriteLine($"Error: {Sessionname}");
            var user = db.Users.FirstOrDefault(x => x.Username == Sessionname);
            Console.WriteLine($"Error: {user.Password}");
           // Console.WriteLine($"Error: {user.Password}");
            if (user != null)
            {
                if (string.IsNullOrWhiteSpace(userChange.Password))
                {
                    ModelState.AddModelError("", "Yêu cầu nhập mật khẩu hiện tại") ;
                    return View(userChange);
                }

                if (!userChange.Password.ToString().Trim().Equals(user.Password.ToString().Trim()) )
                //if(!BCrypt.Net.BCrypt.Verify(userChange.Password.Trim(), user.Password.Trim()))
                {
                    Console.WriteLine($"So sánh kiểu: {user.Password.GetType()}      {userChange.Password.GetType()}");
                    ModelState.AddModelError("", "Sai mật khẩu!");
                    
                    return View(userChange);
                }

                if (string.IsNullOrWhiteSpace(userChange.PasswordChange1) || string.IsNullOrWhiteSpace(userChange.PasswordChange2))
                {
                    ModelState.AddModelError("", "Yêu cầu nhập đủ");
                    return View(userChange);
                }

                if (userChange.PasswordChange1 != userChange.PasswordChange2)
                {
                    ModelState.AddModelError("", "Bạn đã nhập sai mật khẩu mới không trùng nhau!");
                    return View(userChange);
                }

                // Cập nhật mật khẩu và lưu vào database
                user.Password = userChange.PasswordChange2;
                db.SaveChanges();
                ModelState.AddModelError("", "Cập nhật mật khẩu thành công!");
                //TempData["Message"] = "Cập nhật mật khẩu thành công!";
               // return RedirectToAction("ChangePasswordConfirmation"); // hoặc action bạn muốn chuyển hướng sau khi cập nhật thành công
               return View();
            }

            ModelState.AddModelError("", "Không tìm thấy người dùng!");
            return View(userChange);
        }
        //public string HashPassword(string password)
        //{
        //    using (var sha256 = SHA256.Create())
        //    {
        //        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        //        StringBuilder builder = new StringBuilder();
        //        for (int i = 0; i < bytes.Length; i++)
        //        {
        //            builder.Append(bytes[i].ToString("x2"));
        //        }
        //        return builder.ToString();
        //    }
        //}

    }
}
