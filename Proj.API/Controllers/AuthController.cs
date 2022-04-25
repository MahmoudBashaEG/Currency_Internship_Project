using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proj.Core.DTOs;
using Proj.Core.Exceptions;
using Proj.Core.Services;

namespace Proj.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public AuthController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDTO  registerDto)
        {
            if (registerDto == null)
                return BadRequest("Please Enter The User Data");

            IdentityResult result;
            try
            {
                result = await _userServices.Register(registerDto);
                if (result.Succeeded)
                    return Ok(result);
                else
                    return BadRequest(result.Errors);
            }
            catch(RepeatedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }           
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (loginDTO == null)
                return BadRequest("You Have to Include the username and password.");

            try
            {
                await _userServices.Login(loginDTO);
                return Ok("You Signedin Successfully");
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (WrongPasswordException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
