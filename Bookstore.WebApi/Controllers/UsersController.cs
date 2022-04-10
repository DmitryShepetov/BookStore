using AutoMapper;
using Bookstore.WebApi.Data.Helpers;
using Bookstore.WebApi.Data.Interface;
using Bookstore.WebApi.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly JwtHandler jwtHandler;
        private readonly IEmailService emailService;

        public UsersController(UserManager<User> userManager, IMapper mapper, JwtHandler jwtHandler, IEmailService emailService)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.jwtHandler = jwtHandler;
            this.emailService = emailService;
        }
        [HttpPost("Registration")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistration userForRegistration)
        {
            if (userForRegistration == null || !ModelState.IsValid)
                return BadRequest();
            var user = mapper.Map<User>(userForRegistration);

            var result = await userManager.CreateAsync(user, userForRegistration.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);

                return BadRequest(new RegistrationResponse { Errors = errors });
            }
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", user.Email }
            };
            var callback = QueryHelpers.AddQueryString(userForRegistration.ClientURI, param);
            var message = new MessageEmail(user.Email, "Email Confirmation token", callback);
            await emailService.SendEmailAsync(message);
            await userManager.AddToRoleAsync(user, "User");
            return StatusCode(201);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthentication userForAuthentication)
        {
            var user = await userManager.FindByNameAsync(userForAuthentication.Email);
            if (user == null)
                return BadRequest("Invalid Request");
            if (!await userManager.IsEmailConfirmedAsync(user))
                return Unauthorized(new AuthResponse { ErrorMessage = "Email is not confirmed" });
            if (!await userManager.CheckPasswordAsync(user, userForAuthentication.Password))
                return Unauthorized(new AuthResponse { ErrorMessage = "Invalid Authentication" });
            if (await userManager.GetTwoFactorEnabledAsync(user))
                return await GenerateOTPFor2StepVerification(user);
            var token = await jwtHandler.GenerateToken(user);
            await userManager.ResetAccessFailedCountAsync(user);
            return Ok(new AuthResponse { IsAuthSuccessful = true, Token = token });
        }
        private async Task<IActionResult> GenerateOTPFor2StepVerification(User user)
        {
            var providers = await userManager.GetValidTwoFactorProvidersAsync(user);
            if (!providers.Contains("Email"))
            {
                return Unauthorized(new AuthResponse { ErrorMessage = "Invalid 2-Step Verification Provider." });
            }
            var token = await userManager.GenerateTwoFactorTokenAsync(user, "Email");
            var message = new MessageEmail(user.Email, "Authentication token", token);
            await emailService.SendEmailAsync(message);
            return Ok(new AuthResponse { Is2StepVerificationRequired = true, Provider = "Email" });
        }
        [HttpGet("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Invalid Email Confirmation Request");
            var confirmResult = await userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                return BadRequest("Invalid Email Confirmation Request");
            return Ok();
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword forgotPassword)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = await userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null)
                return BadRequest("Invalid Request");
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", forgotPassword.Email }
            };
            var callback = QueryHelpers.AddQueryString(forgotPassword.ClientURI, param);
            var message = new MessageEmail(user.Email, "Reset password token", callback);
            await emailService.SendEmailAsync(message);
            return Ok();
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = await userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");
            var resetPassResult = await userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);
                return BadRequest(new { Errors = errors });
            }
            return Ok();
        }
        [HttpPost("TwoStepVerification")]
        public async Task<IActionResult> TwoStepVerification([FromBody] TwoFactor twoFactorDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var user = await userManager.FindByEmailAsync(twoFactorDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");
            var validVerification = await userManager.VerifyTwoFactorTokenAsync(user, twoFactorDto.Provider, twoFactorDto.Token);
            if (!validVerification)
                return BadRequest("Invalid Token Verification");
            var token = await jwtHandler.GenerateToken(user);
            return Ok(new AuthResponse { IsAuthSuccessful = true, Token = token });
        }
    }
}
