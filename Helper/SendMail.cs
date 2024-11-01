using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Linq;
namespace QLBN.Helper
{
    public class SendMail
    {
        public static bool SendEmail(string to, string subject, string body, string attachFile)
        {
            try
            {
                // Tạo đối tượng MailMessage với các thông tin người gửi, người nhận, tiêu đề và nội dung email
                MailMessage msg = new MailMessage(ConstantHelper.emailSender, to, subject, body);

                // Tạo đối tượng SmtpClient để kết nối với máy chủ SMTP
                using (var client = new SmtpClient(ConstantHelper.hostEmail, ConstantHelper.portEmail))
                {
                    client.EnableSsl = true; // Kích hoạt SSL để bảo mật

                    // Kiểm tra nếu có tệp đính kèm, thì thêm vào email
                    //if (!string.IsNullOrEmpty(attachFile))
                    //{
                    //    Attachment attachment = new Attachment(attachFile);
                    //    msg.Attachments.Add(attachment);
                    //}

                    //// Thiết lập thông tin đăng nhập (email và mật khẩu của người gửi)
                    NetworkCredential credential = new NetworkCredential(
                    ConstantHelper.emailSender, ConstantHelper.passwordSender);

                    client.UseDefaultCredentials = false; // Không sử dụng thông tin đăng nhập mặc định
                    client.Credentials = credential; // Thiết lập thông tin đăng nhập

                    client.Send(msg); // Gửi email
                }
            }
            catch (Exception)
            {
                return false; // Nếu có lỗi xảy ra, trả về false
            }

            return true; // Nếu gửi thành công, trả về true
        }

    }
}
