using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyFirstApi.Dto;
using MyFirstApi.GenericResponse;
using MyFirstApi.IService;

namespace MyFirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            try
            {
                var result = await _authService.LoginUser(userDto);

                if (result.Item1 == 0)
                {
                    return NotFound(ResponseResult<TokenDto>.Failure(result.Item2, result.Item2.Message));
                }

                if (result.Item1 == 1)
                {
                    return BadRequest(ResponseResult<TokenDto>.Failure(result.Item2, result.Item2.Message));
                }

                return Ok(ResponseResult<TokenDto>.Success(result.Item2, result.Item2.Message));

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            try
            {
                var result = await _authService.RegisterUser(userDto);

                if (result.Item1 == 0)
                {
                    return Ok(ResponseResult<string>.Failure(null, result.Item2));
                }

                return Ok(ResponseResult<string>.Success(null, result.Item2));
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost("GenerateQrCode")]
        public async Task<IActionResult> GenerateQrCode(string email)
        {
            try
            {
                var result = await _authService.GenerateQrCode(email);
                if (string.IsNullOrEmpty(result))
                {
                    return NotFound(ResponseResult<string>.Failure(null, "User Not Found"));
                }
                return Ok(ResponseResult<string>.Success(result, "QR Code Generated Successfully!"));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("VerifyOTP")]
        public async Task<IActionResult> VerifyOTP(string email, string otp)
        {
            try
            {
                var result = await _authService.VerifyOTP(email, otp);
                if (string.IsNullOrEmpty(result))
                {
                    return NotFound(ResponseResult<string>.Failure(null, "User Not Found"));
                }
                if (result == "Invalid OTP")
                {
                    return BadRequest(ResponseResult<string>.Failure(null, "Invalid OTP"));
                }
                return Ok(ResponseResult<string>.Success(result, "OTP Verified Successfully!"));
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
