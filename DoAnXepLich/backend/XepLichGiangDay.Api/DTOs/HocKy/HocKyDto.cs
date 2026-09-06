namespace XepLichGiangDay.Api.DTOs.HocKy;

public class HocKyDto
{
    public int MaHocKy { get; set; }
    public int MaNamHoc { get; set; }
    public string? TenNamHoc { get; set; }
    public string TenHocKy { get; set; } = string.Empty;
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public string TrangThai { get; set; } = string.Empty;
}
