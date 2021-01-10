using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Szlaki.Models;

namespace Szlaki.Services.User
{
    public interface IUserService
    {
        Task<ApplicationUser> GetCurrentUserAsync();
        Task<string> GetCurrentUserIdAsync();
        IEnumerable<Claim> GetClaims();
        Claim GetNameIdentifierClaim();
    }
}
