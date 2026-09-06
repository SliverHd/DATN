using Microsoft.AspNetCore.Mvc;

namespace XepLichGiangDay.Api.Controllers;

[ApiController]
[Route("api/nguyen-vong")]
public class NguyenVongController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(Array.Empty<object>());
    }
}
