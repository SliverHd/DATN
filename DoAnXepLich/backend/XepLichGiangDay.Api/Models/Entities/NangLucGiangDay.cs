namespace XepLichGiangDay.Api.Models.Entities;

public class NangLucGiangDay
{
    public int MaNangLuc { get; set; }
    public int MaGiangVien { get; set; }
    public int MaHocPhan { get; set; }
    public int MucDoChuyenMon { get; set; }
    public string TrangThai { get; set; } = "DangApDung";
}

