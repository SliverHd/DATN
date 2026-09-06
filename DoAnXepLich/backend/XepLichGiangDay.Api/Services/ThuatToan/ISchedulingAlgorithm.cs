using XepLichGiangDay.Api.DTOs.XepLich;

namespace XepLichGiangDay.Api.Services.ThuatToan;

public interface ISchedulingAlgorithm
{
    string TenThuatToan { get; }
    Task<KetQuaXepLichDto> ChayAsync(DuLieuXepLichDto duLieu, CancellationToken cancellationToken = default);
}
