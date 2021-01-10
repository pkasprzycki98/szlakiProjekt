using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Szlaki.Contracts.Response;
using Szlaki.Models;
using Szlaki.Request;
using Szlaki.Services.Interfaces;
using Szlaki.Shared;
using System;
using System.Threading.Tasks;

namespace Szlaki.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;
        private readonly IRegisterService _registerService;
        private readonly UserManager<ApplicationUser> _userManager;
        public LoginController(ILoginService loginService, IRegisterService registerService, UserManager<ApplicationUser> userManager)
        {
            _loginService = loginService;
            _registerService = registerService;
            _userManager = userManager;
        }

        [HttpPost("/login")]  
        public async Task<IActionResult> Login(LoginRequest _loginRequest)
        {
            var response = await _loginService.LoginAsync(_loginRequest.Username, _loginRequest.Password);

            if(response.Success == false)
            {
                return BadRequest(new AuthFailedResponse { message = "zły login lub haslo" });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = response.Token,
                RefreshToken = response.RefreshToken
            });
        }
        [HttpPost("/refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequest _refreshTokenRequest)
        {
            var response = await _loginService.RefreshTokenAsync(_refreshTokenRequest.Token, _refreshTokenRequest.RefreshToken);


            if (response.Success == false)
            {
                return BadRequest(response);
            }

            return Ok(new AuthSuccessResponse
            {
                Token = response.Token,
                RefreshToken = response.RefreshToken
            });
        }
    }
}
