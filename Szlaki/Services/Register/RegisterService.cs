using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IIS.Core;
using Szlaki.DbContext;
using Szlaki.Models;
using Szlaki.Models.ViewModels;
using Szlaki.Services.Interfaces;
using Szlaki.Shared;
using Szlaki.Shared.MailService;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace Szlaki.Services.Register
{
    public class RegisterService : IRegisterService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMailService _mailSerivce;

        private readonly string _emailPasswordResetTokenPath = Path.Combine("HTMLTemplates", "PasswordResetEmailTemplate.html");
        private readonly string _resetPasswordMailSubject = "Salon ptyczny - resetowanie hasła";
        public RegisterService(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, IMailService mailSerivce)
        {
            _userManager = userManager;
            _dbContext = dbContext;
            _roleManager = roleManager;
            _mailSerivce = mailSerivce;
        }

        public async Task<ServiceResponse<string>> RegisterAsync(RegisterRequest model)
        {
            var role = "Worker";
            var response = new ServiceResponse<string>();
            var user = await _userManager.FindByNameAsync(model.Username);

            if(user == null && model.Username.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(model.Email);
            }
            if(user != null)
            {
                response.Success = false;
                response.Message = "Nie udało się utworzyć użytkownika.";

                return response;
            }

            var now = DateTime.Now;

            user = new ApplicationUser()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                Email = model.Email,
                CraeteDateTime = now,
                LastModifedDateTime = now
            };
            await _userManager.CreateAsync(user, model.Password);
               
            if (await _roleManager.RoleExistsAsync(role))
            {
                await _userManager.AddToRoleAsync(user, role);
            }
           
            await _dbContext.SaveChangesAsync();

            response.Success = true;
            response.Data = user.UserName;
           
            return response;
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest resetPasswordViewModel)
        {
            var user =  await _userManager.FindByIdAsync(resetPasswordViewModel.UserId);
            if(user == null)
            {
                throw new ValidationException("Nie znaleziono użytkownika");
            }

           await _userManager.ChangePasswordAsync(user, resetPasswordViewModel.OldPassword, resetPasswordViewModel.NewPassword);
        }
        public async Task SendPasswordResetTokenAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
               
                throw new ValidationException("Nie znaleziono użytkownika");
            }


            var passwordTokenEncodedUrl = await GenerateResetPasswordTokenAsync(user);
            var mailTemplate = File.ReadAllText(_emailPasswordResetTokenPath);
            var templateParameters = new
            {
                resetPasswordUrl = $"/{user.Id}/{passwordTokenEncodedUrl}"
            };
            var recipients = new[] { user.Email };


            _mailSerivce.Send(_resetPasswordMailSubject, mailTemplate, templateParameters, recipients);
        }

        public async Task<string> GenerateResetPasswordTokenAsync(ApplicationUser applicationUser)
        {
            var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);

            var passwordResetTokenUrlEncoded = HttpUtility.UrlEncode(passwordResetToken);

            return passwordResetTokenUrlEncoded;
        }
    }
}
