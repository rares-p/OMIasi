using Application.Contracts.Identity;
using Application.Models.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class AuthenticationController(IAuthService authService) : Controller
{
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegistrationModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid payload");
            }

            var authResult = await authService.Registration(model);

            if (!authResult.IsSuccess)
            {
                if (authResult.Value != null!)
                    await authService.DeleteUser(authResult.Value);
                return BadRequest(authResult.Error);
            }

            return CreatedAtAction(nameof(RegisterUser), model);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid payload");
            }

            var message = await authService.Login(model);

            return Ok(message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}