namespace XepLichGiangDay.Api.DTOs.NguyenVong;

public class NguyenVongDto
{
    public int MaNguyenVong { get; set; }
    public int MaGiangVien { get; set; }
    public string TenGiangVien { get; set; } = string.Empty;
    public int MaHocKy { get; set; }
    public string LoaiNguyenVong { get; set; } = string.Empty;
    public int MucDo { get; set; }
    public int? Thu { get; set; }
    public int? TietBatDau { get; set; }
    public int? TietKetThuc { get; set; }
    public double TrongSoPhat { get; set; }
    public string? NoiDungGoc { get; set; }
}

public class TaoNguyenVongDto
{
    public int MaGiangVien { get; set; }
    public int MaHocKy { get; set; }
    public string LoaiNguyenVong { get; set; } = "ThoiGianDay"; // "ThoiGianDay", "MonGiangDay"
    public int MucDo { get; set; } = 3; // 1 (Min) -> 5 (Max)
    public int? Thu { get; set; }
    public int? TietBatDau { get; set; }
    public int? TietKetThuc { get; set; }
    public double TrongSoPhat { get; set; } = 10.0;
    public string? NoiDungGoc { get; set; }
}
