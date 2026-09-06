using Microsoft.AspNetCore.Mvc;
using XepLichGiangDay.Api.DTOs.Auth;
using XepLichGiangDay.Api.Services;

namespace XepLichGiangDay.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(AuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<ThongTinDangNhapDto>> Login(DangNhapDto dto)
    {
        var result = await authService.DangNhapAsync(dto);
        if (result is null)
        {
            return Unauthorized("Ten dang nhap hoac mat khau khong dung, hoac tai khoan da bi khoa.");
        }
        return Ok(result);
    }

    [HttpPost("doi-mat-khau")]
    public async Task<IActionResult> DoiMatKhau(DoiMatKhauDto dto)
    {
        var success = await authService.DoiMatKhauAsync(dto);
        return success ? Ok(new { message = "Doi mat khau thanh cong." }) : BadRequest("Mat khau cu khong dung hoac thong tin khong hop le.");
    }
}
