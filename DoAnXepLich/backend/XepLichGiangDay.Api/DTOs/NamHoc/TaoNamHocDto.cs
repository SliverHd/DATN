namespace XepLichGiangDay.Api.DTOs.NamHoc;

public class TaoNamHocDto
{
    public string TenNamHoc { get; set; } = string.Empty;
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public string TrangThai { get; set; } = "DangApDung";
}
