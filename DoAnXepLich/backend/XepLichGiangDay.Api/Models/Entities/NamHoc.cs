namespace XepLichGiangDay.Api.Models.Entities;

public class NamHoc
{
    public int MaNamHoc { get; set; }
    public string TenNamHoc { get; set; } = string.Empty;
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayKetThuc { get; set; }
    public string TrangThai { get; set; } = "DangApDung";
}

