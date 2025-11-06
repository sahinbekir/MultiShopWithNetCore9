using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiShopWithNetCore9.IdentityServer.Dtos;
using MultiShopWithNetCore9.IdentityServer.Models;

namespace MultiShopWithNetCore9.IdentityServer.Controllers
{
    [Authorize(Policy = IdentityServerConstants.LocalApi.PolicyName)]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> UserRegister(UserRegisterDto user)
        {
            var value = new ApplicationUser()
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                //PasswordHash = user.Password,
                Name = user.Name,
                Surname = user.Surname
            };
            var result = await _userManager.CreateAsync(value, user.Password);

            if (result.Succeeded)
            {
                return Ok("Succed");

            }
            else
            {
                return BadRequest("Failed" + result.Errors.Select(e => e.Description));
            }
        }
    }
}
