namespace XepLichGiangDay.Api.Models.Entities;

public class PhanCongGiangDay
{
    public int MaPhanCong { get; set; }
    public int MaLopHocPhan { get; set; }
    public int MaGiangVien { get; set; }
    public int MaHocKy { get; set; }
    public string NguonPhanCong { get; set; } = string.Empty;
    public double DiemPhat { get; set; }
    public string TrangThai { get; set; } = "DuKien";
}

