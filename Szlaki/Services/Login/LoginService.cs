using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Szlaki.Shared;
using Szlaki.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Szlaki.Services.Interfaces;
using Szlaki.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Szlaki.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings; 
        private readonly IRegisterService _registerService;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ApplicationDbContext _applicationDbContext;

        public LoginService(IConfiguration configuration, UserManager<ApplicationUser> userManager, IRegisterService registerService, TokenValidationParameters tokenValidationParameters, JwtSettings jwtSettings,ApplicationDbContext applicationDbContext)
        {
            _configuration = configuration;
            _userManager = userManager;
            _registerService = registerService;
            _tokenValidationParameters = tokenValidationParameters;
            _jwtSettings = jwtSettings;
            _applicationDbContext = applicationDbContext;
        }
        public async Task<AuthenticationResult> LoginAsync(string username, string password )
        {
            var response = new AuthenticationResult();
            var User = await _userManager.FindByNameAsync(username);
            if (User == null || !await _userManager.CheckPasswordAsync(User, password))
            {
                throw new UnauthorizedAccessException("Błędny login lub hasło");
            }
            else
            {              
                response = await this.CreateToken(User);            
            }

            return response;            
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {          
            var validatedToken = GetPrincipalFromToken(token);
            if (validatedToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "Invalid token" } };
            }
            var expiredDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiredDateUnix);

            if(expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult { Errors = new[] { "This token hasn't expired yet" } };
            }
            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedRefreshToken = await _applicationDbContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token dosn't exist" } };
            }
            if (DateTime.UtcNow > storedRefreshToken.ExpiredTime)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token has expired" } };
            }
            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh has been invalidated" } };
            }
            if (storedRefreshToken.Used)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh has been used" } };
            }
            if(storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh doesn't match this JWT" } };
            }
            storedRefreshToken.Used = true;
            _applicationDbContext.RefreshTokens.Update(storedRefreshToken);
            await _applicationDbContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);

            return await CreateToken(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature,
                       StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<AuthenticationResult> CreateToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
           
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenTimeLife),
                SigningCredentials = creds
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationTime = DateTime.UtcNow,
                ExpiredTime = DateTime.UtcNow.AddMonths(6),
            };
            await _applicationDbContext.RefreshTokens.AddAsync(refreshToken);
            await _applicationDbContext.SaveChangesAsync();


            return new AuthenticationResult { Token = tokenHandler.WriteToken(token), Success = true, RefreshToken = refreshToken.Token};
        }
    }
}
