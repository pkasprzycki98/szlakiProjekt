using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Szlaki.Models;
using Szlaki.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Szlaki.Services.User
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userClaims = GetClaims().ToList();

            if (userClaims.Any())
            {
                var name = userClaims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                return await _userManager.FindByNameAsync(name);
            }

            return null;
        }

        public async Task<string> GetCurrentUserIdAsync()
        {
            var userClaims = GetClaims().ToList();

            if (userClaims.Any())
            {
                var name = userClaims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
                var user =  await _userManager.FindByNameAsync(name);
                return user.Id;
            }
            return "";
        }

        public IEnumerable<Claim> GetClaims()
        {
            var claims = _httpContextAccessor.HttpContext?.User?.Claims;

            if (claims == null)
                return Enumerable.Empty<Claim>();

            return claims;
        }

        public Claim GetNameIdentifierClaim()
        {
            var response = new ServiceResponse<string>();
            return GetClaims().First(c => c.Type == ClaimTypes.NameIdentifier);
        }
    }
}
