namespace XepLichGiangDay.Api.DTOs.HocKy;

public class TaoHocKyDto
{
    public int MaNamHoc { get; set; }
    public string TenHocKy { get; set; } = string.Empty;
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public string TrangThai { get; set; } = "DangApDung";
}
