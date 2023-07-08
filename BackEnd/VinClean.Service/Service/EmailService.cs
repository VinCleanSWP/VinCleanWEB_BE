using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using VinClean.Service.DTO.Email;
using VinClean.Repo.Repository;
using VinClean.Service.DTO;
using Azure;
using VinClean.Service.DTO.CustomerResponse;
using VinClean.Repo.Models;

namespace VinClean.Service.Service
{
    public interface IEmailService
    {
        void SendEmail(EmailFormDTO request);
        Task<ServiceResponse<AccountDTO>> SendEmailVerify(EmailFormDTO request);
        Task<ServiceResponse<AccountDTO>> SendEmailResetPassword(EmailFormDTO request);
        Task<ServiceResponse<AccountDTO>> SendAssignToCustomer(EmailFormDTO request);
        Task<ServiceResponse<AccountDTO>> SendAssignToEmployee(EmailFormDTO request);
        Task<ServiceResponse<AccountDTO>> SendEmailToChangeEmployee(EmailFormDTO request);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly IAccountRepository _Accrepository;
        private readonly IProcessRepository _process;
        public EmailService(IConfiguration config, IAccountRepository Accrepository, IProcessRepository process)
        {
            _config = config;
            _Accrepository = Accrepository;
            _process = process;
        }

        public void SendEmail(EmailFormDTO request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public async Task<ServiceResponse<AccountDTO>> SendEmailVerify(EmailFormDTO request)
        {
            ServiceResponse<AccountDTO> _response = new();
            try
            {
                var checkemail = await _Accrepository.GetbyEmail(request.To);
                if (checkemail == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(checkemail.Email));
                email.Subject = "Verification Account";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<p>Hello <i>{checkemail.Name}</i></p>" +
                           $"<p>You registered an account on {checkemail.Email}, before being able to use your account you need to verify that this is your email address by clicking here: </p>" +
                           $"<p>https://localhost:7013/api/Account/Verify?token={checkemail.VerificationToken}</p>" +
                           $"<p>Or you can copy this Token and paste in Token form : </p>" +
                           $"<p><b>{checkemail.VerificationToken}</b></p>" +
                           "<p>Kind Regards, <h3>VinClean</h3></p>"
                };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                _response.Success = true;
                _response.Message = "OK";
                _response.Data = null;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        /*public async void SendEmailResetPassword(string Accountemail)
        {
            var checkemail = await _Accrepository.GetbyEmail(Accountemail);
            if (checkemail != null)
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(Accountemail));
                email.Subject = "Reset Password";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = "Hello [name]," +
                    $"\r\n\r\nSomebody requested a new password for the {checkemail.Name} account associated with {checkemail.Email}." +
                    "\r\n\r\nNo changes have been made to your account yet." +
                    "\r\n\r\nYou can reset your password by clicking the link below:" +
                    "\r\n\r\nOr Use your secret code!:" +
                    $"\r\n\r\n{checkemail.PasswordResetToken}" +
                    "\r\n\r\nIf you did not request a new password, please let us know immediately by replying to this email." +
                    "\r\n\r\nYours,\r\nThe VinClean team"
                };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }*/

        public async Task<ServiceResponse<AccountDTO>> SendEmailResetPassword(EmailFormDTO request)
        {
            ServiceResponse<AccountDTO> _response = new();
            try
            {
                var checkemail = await _Accrepository.GetbyEmail(request.To);
                if (checkemail == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(checkemail.Email));
                email.Subject = "Verification Account";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<p>Hello <i>{checkemail.Name}</i>,</p>" +
                    $"<p>Somebody requested a new password for the {checkemail.Name} account associated with {checkemail.Email}.</p>" +
                    "<p>No changes have been made to your account yet.</p>" +
                    "<p>You can reset your password by clicking the link below:</p>" +
                    "<p>Or Use your secret code!:</p>" +
                    $"<p><b>{checkemail.PasswordResetToken}</b></p>" +
                    "<p>If you did not request a new password, please let us know immediately by replying to this email.</p>" +
                    "<p>Yours,<h3>The VinClean team</h3></p>"
                };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                _response.Success = true;
                _response.Message = "OK";
                _response.Data = null;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }


        public async Task<ServiceResponse<AccountDTO>> SendAssignToCustomer(EmailFormDTO request)
        {
            ServiceResponse<AccountDTO> _response = new();
            try
            {
                var checkemail = await _Accrepository.GetbyEmail(request.To);
                var process = await _process.GetAllInfoById(request.ProcessId);
                if (checkemail == null && process == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(checkemail.Email));
                email.Subject = "Thông báo dịch vụ VinClean";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<div><p>Xin chào <i>{checkemail.Name}</i>,</p>" +
                    $"<p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</p>" +
                    "<p>Chúng tôi gửi lại bạn thông tin Dịch vụ và Nhân viên:</p>" +
                    "<h4>Thông tin Khách Hàng:</h4>" +
                    $"<p><b>Tên</b>: {checkemail.Name} </p>" +
                    $"<p><b>Điện thoại</b>: {process.Phone} </p>" +
                    $"<p><b>Địa chỉ</b>: {process.Address} </p>" +
                    $"<p><b>Ngày Làm</b>: {process.Date.ToString("dd/MM/yyyy")}</p>" +
                    $"<p><b>Giờ Làm</b>: {process.StartTime} - {process.EndTime}</p>" +
                    "<h4>Thông tin Nhân viên:</h4>" +
                    $"<p><b>Tên Nhân Viên</b>: {process.EmployeeName} </p>" +
                    $"<p><b>Điện thoại</b>: {process.EmployeePhone} </p>" +
                    $"<p><b>Email</b>: {process.EmployeeEmail} </p>" +
                    $"<p><b>Xin vui lòng hãy đến đúng giờ. </p>" +
                    "<p>Nếu bạn có bất cứ phàn nàn nào hãy gửi email phản hồi tới địa chỉ Email: vincleanvhgp@gmail.com " +
                    "<p>hoặc Hotline: 0329 300 159.</p>" +
                    "<p>Chúng tôi sẽ phản hồi trong thờ gian sớm nhất.</p>" +
                    "<p><h3>The VinClean team</h3></p </div>"
                };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                _response.Success = true;
                _response.Message = "OK";
                _response.Data = null;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }

        public async Task<ServiceResponse<AccountDTO>> SendAssignToEmployee(EmailFormDTO request)
        {
            ServiceResponse<AccountDTO> _response = new();
            try
            {
                var checkemail = await _Accrepository.GetbyEmail(request.To);
                var process = await _process.GetAllInfoById(request.ProcessId);
                if (checkemail == null && process == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(checkemail.Email));
                email.Subject = "THÔNG BÁO CÔNG VIỆC";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<p>Thông báo tới nhân viên <i>{checkemail.Name}</i>,</p>" +
                    $"<p>Bạn có một công việc mới.</p>" +
                    "<p>Chúng tôi gửi lại bạn thông tin Dịch vụ và Khác Hàng:</p>" +
                    "<h4>Thông tin Khách Hàng:</h4>" +
                    $"<p><b>Tên</b>: {process.Name} </p>" +
                    $"<p><b>Điện thoại</b>: {process.Phone} </p>" +
                    $"<p><b>Địa chỉ</b>: {process.Address} </p>" +
                    $"<p><b>Ngày Làm</b>: {process.Date.ToString("dd/MM/yyyy")}</p>" +
                    $"<p><b>Giờ Làm</b>: {process.StartTime} - {process.EndTime}</p>" +
                    "<h4>Thông tin Nhân viên:</h4>" +
                    $"<p><b>Tên Nhân Viên</b>: {process.EmployeeName} </p>" +
                    $"<p><b>Điện thoại</b>: {process.EmployeePhone} </p>" +
                    $"<p><b>Email</b>: {process.EmployeeEmail} </p>" +
                    "<p>Nếu bạn có bất cứ phàn nàn nào hãy gửi email phản hồi tới địa chỉ Email: vincleanvhgp@gmail.com hoặc Hotline: 0329 300 159." +
                    "Chúng tôi sẽ phản hồi trong thờ gian sớm nhất.</p>" +
                    "<p>Yours,<h3>The VinClean team</h3></p>"
                };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                _response.Success = true;
                _response.Message = "OK";
                _response.Data = null;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }


        public async Task<ServiceResponse<AccountDTO>> SendEmailToChangeEmployee(EmailFormDTO request)
        {
            ServiceResponse<AccountDTO> _response = new();
            try
            {
                var checkemail = await _Accrepository.GetbyEmail(request.To);
                var process = await _process.GetAllInfoById(request.ProcessId);
                if (checkemail == null && process == null)
                {
                    _response.Success = false;
                    _response.Message = "NotFound";
                    return _response;
                }
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
                email.To.Add(MailboxAddress.Parse(checkemail.Email));
                email.Subject = "THÔNG BÁO CẬP NHẬT NHÂN VIÊN";
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = $"<p>Xin chào <i>{checkemail.Name}</i>,</p>" +
                    $"<p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</p>" +
                    "<p>Chúng tôi gửi lại bạn thông tin Dịch vụ và Nhân viên:</p>" +
                    "<h4>Thông tin Khách Hàng:</h4>" +
                    $"<p><b>Tên</b>: {checkemail.Name} </p>" +
                    $"<p><b>Điện thoại</b>: {process.Phone} </p>" +
                    $"<p><b>Địa chỉ</b>: {process.Address} </p>" +
                    $"<p><b>Ngày Làm</b>: {process.Date.ToString("dd/MM/yyyy")}</p>" +
                    $"<p><b>Giờ Làm</b>: {process.StartTime} - {process.EndTime}</p>" +
                    "<h4>Thông tin Nhân viên:</h4>" +
                    $"<p><b>Tên Nhân Viên</b>: {process.EmployeeName} </p>" +
                    $"<p><b>Điện thoại</b>: {process.EmployeePhone} </p>" +
                    $"<p><b>Email</b>: {process.EmployeeEmail} </p>" +
                    $"<p><b>Xin vui lòng hãy đến đúng giờ. </p>" +
                    "<p>Nếu bạn có bất cứ phàn nàn nào hãy gửi email phản hồi tới địa chỉ Email: vincleanvhgp@gmail.com " +
                    "<p>hoặc Hotline: 0329 300 159.</p>" +
                    "<p>Chúng tôi sẽ phản hồi trong thờ gian sớm nhất.</p>" +
                    "<p><h3>The VinClean team</h3></p>"
                };

                using var smtp = new SmtpClient();
                smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
                smtp.Send(email);
                smtp.Disconnect(true);

                _response.Success = true;
                _response.Message = "OK";
                _response.Data = null;
            }
            catch (Exception ex)
            {
                _response.Success = false;
                _response.Message = "Error";
                _response.Data = null;
                _response.ErrorMessages = new List<string> { Convert.ToString(ex.Message) };
            }
            return _response;
        }
    }
}
