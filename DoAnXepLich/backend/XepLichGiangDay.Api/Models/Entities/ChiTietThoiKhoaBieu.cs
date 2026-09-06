namespace XepLichGiangDay.Api.Models.Entities;

public class ChiTietThoiKhoaBieu
{
    public int MaChiTiet { get; set; }
    public int MaLopHocPhan { get; set; }
    public int Thu { get; set; }
    public int TietBatDau { get; set; }
    public int TietKetThuc { get; set; }
    public string PhongHoc { get; set; } = string.Empty;
    public int TuTuan { get; set; }
    public int DenTuan { get; set; }
}

