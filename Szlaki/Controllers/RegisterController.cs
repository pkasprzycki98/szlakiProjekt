using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Szlaki.Models;
using Szlaki.Models.ViewModels;
using Szlaki.Services.Interfaces;
using Szlaki.Shared.MailService;

namespace Szlaki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {
        private readonly IRegisterService _registerService;
        private readonly IMailService _mailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterController(IRegisterService registerService, IMailService mailService, UserManager<ApplicationUser> userManager)
        {
            _registerService = registerService;
            _mailService = mailService;
            _userManager = userManager;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Post([FromBody] RegisterRequest model)
        {
           var response =  await _registerService.RegisterAsync(model);
            if (response.Success == false)
            {
                
                return NotFound();
            }          
            return Ok(response);                   
        }
        [HttpPost("sendResetPasswordEmail")]
        public async Task<IActionResult> SendPasswordResetEmail([FromBody]PasswordResetTokenViewModel passwordResetTokenDto)
        {
            var user =  await _userManager.FindByIdAsync(passwordResetTokenDto.UserId);
            if(user == null)
            {
                return NotFound("Nie znalezino użytkownika");
            }
           await _registerService.SendPasswordResetTokenAsync(user.Id);

            return Ok();
        }
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordViewModel )
        {
            var user = await _userManager.FindByIdAsync(resetPasswordViewModel.UserId);
            if(user == null)
            {
                return NotFound("Nie znaleziono użytkownika");
            }

            await _registerService.ResetPasswordAsync(resetPasswordViewModel);

            return Ok();
        }


    }
}
