using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Security.Principal;
using VinClean.Repo.Models;
using VinClean.Service.DTO;
using VinClean.Service.DTO.Email;
using VinClean.Service.Service;

namespace VinClean.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPost]
        public IActionResult SendEmail(EmailFormDTO request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }
        [HttpPost("VerifyAccount")]
        public async Task<ActionResult<Account>> SendEmailVerify(EmailFormDTO email)
        {
            var newAccount = await _emailService.SendEmailVerify(email);
            if (newAccount.Success == false && newAccount.Message == "Exist")
            {
                return Ok(newAccount);
            }

            if (newAccount.Success == false && newAccount.Message == "RepoError")
            {
                ModelState.AddModelError("", $"Some thing went wrong in respository layer when adding Account ");
                return StatusCode(500, ModelState);
            }

            if (newAccount.Success == false && newAccount.Message == "Error")
            {
                ModelState.AddModelError("", $"Some thing went wrong in service layer when adding Account ");
                return StatusCode(500, ModelState);
            }
            return Ok(newAccount.Data);
        }

        [HttpPost("ResetPassword")]
        public IActionResult SendEmailResetPassword(string email)
        {
            _emailService.SendEmailResetPassword(email);
            return Ok();
        }
    }
}
