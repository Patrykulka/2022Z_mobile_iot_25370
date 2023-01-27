using ArtistHaven.API.Models;
using ArtistHaven.API.Services;
using ArtistHaven.Shared;
using ArtistHaven.Shared.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArtistHaven.API.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IJwtTokenService jwtTokenService) {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        async public Task<ActionResult<AuthResponse>> Register(RegisterModel userRegister) {
            if (!ModelState.IsValid)
                return BadRequest(new AuthResponse() {
                    Message = "Invalid data",
                    IsSuccessfull = false,
                    Errors = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                });
            
            if (userRegister.Password != userRegister.ConfirmPassword)
                return BadRequest(new AuthResponse() {
                    Errors = new List<string>(){ "Passwords do not match." },
                    IsSuccessfull = false
                });

            User user = new User() {
                UserName = userRegister.Username,
                Email = userRegister.Email,
            };

            IdentityResult result = await _userManager.CreateAsync(user, userRegister.Password);
            if (result.Succeeded) {
                return Ok(new AuthResponse() {
                    IsSuccessfull = true,
                });
            }

            return BadRequest(new AuthResponse() {
                IsSuccessfull = false,
                Errors = result.Errors.Select(err => err.Description)
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Login(LoginModel loginModel) {
            if (!ModelState.IsValid)
                return BadRequest(new AuthResponse() {
                    Message = "Invalid data",
                    IsSuccessfull = false,
                    Errors = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                });

            AuthResponse invalidCredentials = new AuthResponse() {
                Message = "Invalid credentials",
                IsSuccessfull = false
            };
            User user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
                return NotFound(invalidCredentials);

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
            if (result.Succeeded)
                return Ok(new AuthResponse() {
                    Message = _jwtTokenService.Generate(user),
                    IsSuccessfull = true
                });

            return NotFound(invalidCredentials);
        }
    }
}
