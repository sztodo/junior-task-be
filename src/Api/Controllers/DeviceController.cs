using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IDeviceService _deviceService;

        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DeviceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var devices = await _deviceService.GetAllAsync();
            return Ok(devices);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(DeviceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var device = await _deviceService.GetByIdAsync(id);
            return device is null ? NotFound($"Device with ID {id} not found.") : Ok(device);
        }

        [HttpGet("user/{userId:int}")]
        [ProducesResponseType(typeof(IEnumerable<DeviceDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var devices = await _deviceService.GetByUserIdAsync(userId);
            return Ok(devices);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DeviceDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateDeviceDto dto)
        {
            var device = await _deviceService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = device.Id }, device);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(DeviceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDeviceDto dto)
        {
            var device = await _deviceService.UpdateAsync(id, dto);
            return Ok(device);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _deviceService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPatch("{id:int}/assign")]
        [ProducesResponseType(typeof(DeviceDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignUser(int id, [FromBody] AssignDeviceDto dto)
        {
            var device = await _deviceService.AssignUserAsync(id, dto);
            return Ok(device);
        }
    }
}
