namespace XepLichGiangDay.Api.DTOs.XepLich;

public class DuLieuXepLichDto
{
    public int MaHocKy { get; set; }
    public string ThuatToan { get; set; } = string.Empty;
    public int? KichThuocQuanThe { get; set; }
    public int? SoTheHe { get; set; }
    public double? TyLeLaiGhep { get; set; }
    public double? TyLeDotBien { get; set; }
}
