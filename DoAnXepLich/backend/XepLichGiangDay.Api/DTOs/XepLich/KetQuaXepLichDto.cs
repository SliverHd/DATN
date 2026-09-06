namespace XepLichGiangDay.Api.DTOs.XepLich;

public class KetQuaXepLichDto
{
    public string ThuatToan { get; set; } = string.Empty;
    public double TongDiemPhat { get; set; }
    public string TrangThai { get; set; } = "DuLieuMau";
    public List<PhanCongDto> DanhSachPhanCong { get; set; } = [];
}
