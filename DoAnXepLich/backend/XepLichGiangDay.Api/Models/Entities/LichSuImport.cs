namespace XepLichGiangDay.Api.Models.Entities;

public class LichSuImport
{
    public int MaLichSu { get; set; }
    public int MaHocKy { get; set; }
    public string TenFile { get; set; } = string.Empty;
    public DateTime NgayImport { get; set; } = DateTime.Now;
    public int TongSoDong { get; set; }
    public int SoDongThanhCong { get; set; }
    public int SoDongLoi { get; set; }
    public string TrangThai { get; set; } = "ThanhCong";
    public string? GhiChu { get; set; }
}
