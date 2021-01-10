using Szlaki.Models;
using Szlaki.Models.ViewModels;
using Szlaki.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Szlaki.Services.Interfaces
{
   public interface IRegisterService
    {
        Task<ServiceResponse<string>> RegisterAsync(RegisterRequest registerViewModel);
        Task ResetPasswordAsync(ResetPasswordRequest resetPasswordViewModel);
        Task SendPasswordResetTokenAsync(string userId);
        Task<string> GenerateResetPasswordTokenAsync(ApplicationUser applicationUser);
   
    }
}
