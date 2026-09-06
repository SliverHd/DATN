namespace XepLichGiangDay.Api.Models.Entities;

public class LichBanGiangVien
{
    public int MaLichBan { get; set; }
    public int MaGiangVien { get; set; }
    public int MaHocKy { get; set; }
    public int Thu { get; set; }
    public int TietBatDau { get; set; }
    public int TietKetThuc { get; set; }
    public DateTime? TuNgay { get; set; }
    public DateTime? DenNgay { get; set; }
    public string? LyDo { get; set; }
}

