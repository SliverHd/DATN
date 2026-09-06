using XepLichGiangDay.Api.DTOs.XepLich;

namespace XepLichGiangDay.Api.Services.ThuatToan.CpSat;

public class CpSatAlgorithmService : ISchedulingAlgorithm
{
    public string TenThuatToan => "CP_SAT";

    public Task<KetQuaXepLichDto> ChayAsync(DuLieuXepLichDto duLieu, CancellationToken cancellationToken = default)
    {
        _ = cancellationToken;

        var ketQua = new KetQuaXepLichDto
        {
            ThuatToan = TenThuatToan,
            TongDiemPhat = 0,
            TrangThai = $"Du lieu mau cho hoc ky {duLieu.MaHocKy}"
        };

        return Task.FromResult(ketQua);
    }
}
