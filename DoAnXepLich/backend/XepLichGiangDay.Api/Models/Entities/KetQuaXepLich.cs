namespace XepLichGiangDay.Api.Models.Entities;

public class KetQuaXepLich
{
    public int MaKetQua { get; set; }
    public int MaHocKy { get; set; }
    public int MaLopHocPhan { get; set; }
    public int MaGiangVien { get; set; }
    public double DiemPhat { get; set; }
    public string GhiChu { get; set; } = string.Empty;
}

