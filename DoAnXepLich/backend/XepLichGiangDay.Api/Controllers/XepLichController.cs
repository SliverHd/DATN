using Microsoft.AspNetCore.Mvc;
using XepLichGiangDay.Api.DTOs.XepLich;
using XepLichGiangDay.Api.Services.ThuatToan;

namespace XepLichGiangDay.Api.Controllers;

[ApiController]
[Route("api/xep-lich")]
public class XepLichController(IEnumerable<ISchedulingAlgorithm> algorithms) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<KetQuaXepLichDto>> ChayXepLich(YeuCauXepLichDto request, CancellationToken cancellationToken)
    {
        var thuatToan = request.ThuatToan.Trim().ToUpperInvariant();
        var algorithm = algorithms.FirstOrDefault(x => x.TenThuatToan == thuatToan);

        if (algorithm is null)
        {
            return BadRequest("Thuat toan chi ho tro GA hoac CP_SAT.");
        }

        var duLieu = new DuLieuXepLichDto
        {
            MaHocKy = request.MaHocKy,
            ThuatToan = thuatToan,
            KichThuocQuanThe = request.KichThuocQuanThe,
            SoTheHe = request.SoTheHe,
            TyLeLaiGhep = request.TyLeLaiGhep,
            TyLeDotBien = request.TyLeDotBien
        };

        var ketQua = await algorithm.ChayAsync(duLieu, cancellationToken);
        return Ok(ketQua);
    }
}
