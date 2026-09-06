namespace XepLichGiangDay.Api.Models.Entities;

public class DinhMucGiangVien
{
    public int MaDinhMuc { get; set; }
    public int MaGiangVien { get; set; }
    public int MaHocKy { get; set; }
    public int SoTietToiThieu { get; set; }
    public int SoTietToiDa { get; set; }
    public string TrangThai { get; set; } = "DangApDung";
}

