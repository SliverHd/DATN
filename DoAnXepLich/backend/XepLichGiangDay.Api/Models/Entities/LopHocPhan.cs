namespace XepLichGiangDay.Api.Models.Entities;

public class LopHocPhan
{
    public int MaLopHocPhan { get; set; }
    public string MaLopHocPhanTruong { get; set; } = string.Empty;
    public int MaHocPhan { get; set; }
    public int MaHocKy { get; set; }
    public string TenLop { get; set; } = string.Empty;
    public int SoLuongSinhVien { get; set; }
    public string TrangThai { get; set; } = "DangMo";
}

