namespace XepLichGiangDay.Api.DTOs.XepLich;

public class PhanCongDto
{
    public int MaLopHocPhan { get; set; }
    public int MaGiangVien { get; set; }
    public double DiemPhat { get; set; }
    public string GhiChu { get; set; } = string.Empty;
}
