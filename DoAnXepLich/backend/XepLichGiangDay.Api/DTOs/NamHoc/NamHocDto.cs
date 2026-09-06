namespace XepLichGiangDay.Api.DTOs.NamHoc;

public class NamHocDto
{
    public int MaNamHoc { get; set; }
    public string TenNamHoc { get; set; } = string.Empty;
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public string TrangThai { get; set; } = string.Empty;
}
