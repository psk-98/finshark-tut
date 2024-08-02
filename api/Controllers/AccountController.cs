using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/controllers")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = new AppUser { UserName = registerDto.UserName, Email = registerDto.Email };


            var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (createUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRolesAsync(appUser, "User");

                if (roleResult.Succeeded)
                    return Ok("User created successfully");
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, roleResult.Errors);

            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, createUser.Errors);
            }
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e);

        }

    }
}
