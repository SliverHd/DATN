namespace XepLichGiangDay.Api.Services.ThuatToan.Genetic;

public class KhoiTaoQuanThe
{
    private readonly Random _random = new();

    public List<CaThe> TaoQuanTheBanDau(
        int kichThuocQuanThe,
        List<int> danhSachMaLop,
        Dictionary<int, List<int>> ungVienTheoLop)
    {
        var quanThe = new List<CaThe>();

        for (int i = 0; i < kichThuocQuanThe; i++)
        {
            var caThe = new CaThe();
            foreach (var maLop in danhSachMaLop)
            {
                int maGv = 0;
                if (ungVienTheoLop.TryGetValue(maLop, out var dsUngVien) && dsUngVien.Count > 0)
                {
                    maGv = dsUngVien[_random.Next(dsUngVien.Count)];
                }

                caThe.DanhSachGen.Add(new Gen
                {
                    MaLopHocPhan = maLop,
                    MaGiangVien = maGv
                });
            }
            quanThe.Add(caThe);
        }

        return quanThe;
    }
}
