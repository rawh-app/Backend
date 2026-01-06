using Microsoft.AspNetCore.Mvc;
using RAWH.BLL.DTOs;
using RAWH.BLL.Interfaces;

namespace RAWH.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService auth;
        private readonly IToken tokenService;
        // Nosssaaaaa
        public AuthController(IAuthService auth, IToken tokenService)
        {
            this.auth = auth;
            this.tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await auth.Register(model);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await auth.Login(model);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
        [HttpPost("send-reset-code")]
        public async Task<IActionResult> SendResetCode([FromForm] string email)
        {
            var result = await auth.SendResetCode(email);
            if (result.message.Contains("successfully"))
                return Ok(result.message);
            return BadRequest(result.message);
        }
        [HttpPost("verify-reset-code")]
        public async Task<IActionResult> VerifyResetCode([FromForm] string code)
        {
            var result = await auth.VerifyResetCode(code);
            if (result.message.Contains("successfully"))
                return Ok(result.message);
            return BadRequest(result.message);
        }
        [HttpPost("set-new-password")]
        public async Task<IActionResult> SetNewPassword([FromForm] NewPasswordDTO model)
        {
            var result = await auth.SetNewPassword(model);
            if (result.message.Contains("successfully"))
                return Ok(result.message);
            return BadRequest(result.message);
        }




    }
}
