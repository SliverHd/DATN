using Microsoft.AspNetCore.Mvc;
using XepLichGiangDay.Api.DTOs.NguyenVong;
using XepLichGiangDay.Api.Services;

namespace XepLichGiangDay.Api.Controllers;

[ApiController]
[Route("api/nguyen-vong")]
public class NguyenVongController(NguyenVongService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<NguyenVongDto>>> GetAll([FromQuery] int? maHocKy, [FromQuery] int? maGiangVien)
    {
        return Ok(await service.GetAllAsync(maHocKy, maGiangVien));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<NguyenVongDto>> GetById(int id)
    {
        var item = await service.GetByIdAsync(id);
        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<NguyenVongDto>> Create([FromBody] TaoNguyenVongDto dto)
    {
        var created = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.MaNguyenVong }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TaoNguyenVongDto dto)
    {
        var success = await service.UpdateAsync(id, dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await service.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}
