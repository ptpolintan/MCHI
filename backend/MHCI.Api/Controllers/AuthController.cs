using MHCI.Api.DTOs.Requests.Users;
using MHCI.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MHCI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService service;

        public AuthController(IUserService service)
        {
            this.service = service;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            var result = service.Authenticate(request.Email, request.Password);

            return result != null ? Ok(result) : Unauthorized();
        }
    }
}
