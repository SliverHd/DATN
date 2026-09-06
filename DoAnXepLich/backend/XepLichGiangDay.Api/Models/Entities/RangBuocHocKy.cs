namespace XepLichGiangDay.Api.Models.Entities;

public class RangBuocHocKy
{
    public int MaRangBuoc { get; set; }
    public int MaHocKy { get; set; }
    public string TenRangBuoc { get; set; } = string.Empty;
    public string LoaiRangBuoc { get; set; } = string.Empty;
    public double TrongSoPhat { get; set; }
    public string? MoTa { get; set; }
}

