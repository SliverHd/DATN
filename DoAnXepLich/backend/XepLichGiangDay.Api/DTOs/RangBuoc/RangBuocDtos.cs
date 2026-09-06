namespace XepLichGiangDay.Api.DTOs.RangBuoc;

public class RangBuocHocKyDto
{
    public int MaRangBuoc { get; set; }
    public int MaHocKy { get; set; }
    public string TenRangBuoc { get; set; } = string.Empty;
    public string LoaiRangBuoc { get; set; } = string.Empty;
    public double TrongSoPhat { get; set; }
    public string? MoTa { get; set; }
}

public class CapNhatRangBuocDto
{
    public double TrongSoPhat { get; set; }
}

public class DinhMucGiangVienDto
{
    public int MaDinhMuc { get; set; }
    public int MaGiangVien { get; set; }
    public string TenGiangVien { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int MaHocKy { get; set; }
    public int SoTietToiThieu { get; set; }
    public int SoTietToiDa { get; set; }
}

public class CapNhatDinhMucDto
{
    public int MaGiangVien { get; set; }
    public int MaHocKy { get; set; }
    public int SoTietToiThieu { get; set; }
    public int SoTietToiDa { get; set; }
}

public class ApDungDinhMucChungDto
{
    public int MaHocKy { get; set; }
    public int SoTietToiThieu { get; set; }
    public int SoTietToiDa { get; set; }
}
