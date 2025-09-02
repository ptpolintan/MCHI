using MHCI.Api.DTOs.Requests.Checkins;
using MHCI.Api.DTOs.Responses;
using MHCI.Application.Interfaces;
using MHCI.Application.Models;
using MHCI.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MHCI.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CheckInsController : Controller
    {
        private readonly ICheckInService service;

        public CheckInsController(ICheckInService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCheckins([FromQuery] int skip, [FromQuery] int take, [FromQuery] DateOnly? from, [FromQuery] DateOnly? to, [FromQuery] int? userId)
        {
            var result = await this.service.GetCheckins(skip, take, from, to, userId, (int)HttpContext.Items["currentuserid"]);
            return Ok(new GetCheckInsResponseDTO { 
                TotalRecords = result.TotalRecords,
                Data = result.Data,
                Message = result.Message,
                Success = result.Success
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCheckinById([FromRoute] int id)
        {
            return Ok(await this.service.GetCheckinById(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCheckin([FromBody] CreateCheckinRequestDTO request)
        {
            return Ok(await this.service.CreateCheckin(new CheckInModel { UserId = request.UserId, Mood = request.Mood, Notes = request.Notes }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCheckin([FromBody] UpdateCheckinRequestDTO request)
        {
            if((int)HttpContext.Items["currentuserid"]! == request.UserId) return Forbid();

            return Ok(await this.service.UpdateCheckin(new CheckInModel { Id = request.Id, UserId = request.UserId, Mood = request.Mood, Notes = request.Notes }));
        }
    }
}
