namespace XepLichGiangDay.Api.Models.Entities;

public class HocPhan
{
    public int MaHocPhan { get; set; }
    public string TenHocPhan { get; set; } = string.Empty;
    public int SoTinChi { get; set; }
    public int? MaBoMon { get; set; }
    public string TrangThai { get; set; } = "DangApDung";
}

