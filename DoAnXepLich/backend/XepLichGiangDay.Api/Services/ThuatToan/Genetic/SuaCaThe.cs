namespace XepLichGiangDay.Api.Services.ThuatToan.Genetic;

public class ThoiGianLop
{
    public int MaLopHocPhan { get; set; }
    public int Thu { get; set; }
    public int TietBatDau { get; set; }
    public int TietKetThuc { get; set; }

    public bool TrungGioVoi(ThoiGianLop other)
    {
        if (Thu != other.Thu) return false;
        return !(TietKetThuc < other.TietBatDau || TietBatDau > other.TietKetThuc);
    }
}

public class SuaCaThe
{
    private readonly Random _random = new();

    /// <summary>
    /// Toán tử sửa cá thể (Repair Operator):
    /// Quét các lớp trùng lịch của cùng một giảng viên và gán lại cho giảng viên khác không bị trùng.
    /// </summary>
    public CaThe SuaNeuViPham(
        CaThe caThe,
        Dictionary<int, List<ThoiGianLop>> thoiGianTheoLop,
        Dictionary<int, List<int>> ungVienTheoLop)
    {
        // Nhóm các lớp theo giảng viên được phân công
        var lopTheoGv = new Dictionary<int, List<int>>();
        foreach (var gen in caThe.DanhSachGen)
        {
            if (gen.MaGiangVien.HasValue && gen.MaGiangVien.Value > 0)
            {
                int gvId = gen.MaGiangVien.Value;
                if (!lopTheoGv.ContainsKey(gvId))
                {
                    lopTheoGv[gvId] = [];
                }
                lopTheoGv[gvId].Add(gen.MaLopHocPhan);
            }
        }

        // Kiểm tra từng giảng viên xem có bị trùng giờ không
        foreach (var (gvId, dsLop) in lopTheoGv)
        {
            if (dsLop.Count <= 1) continue;

            for (int i = 0; i < dsLop.Count; i++)
            {
                for (int j = i + 1; j < dsLop.Count; j++)
                {
                    int lop1 = dsLop[i];
                    int lop2 = dsLop[j];

                    if (thoiGianTheoLop.TryGetValue(lop1, out var tg1) &&
                        thoiGianTheoLop.TryGetValue(lop2, out var tg2))
                    {
                        bool isConflict = tg1.Any(t1 => tg2.Any(t2 => t1.TrungGioVoi(t2)));
                        if (isConflict)
                        {
                            // Tìm cách đổi giảng viên cho lớp 2
                            var gen2 = caThe.DanhSachGen.FirstOrDefault(g => g.MaLopHocPhan == lop2);
                            if (gen2 != null && ungVienTheoLop.TryGetValue(lop2, out var dsUngVien))
                            {
                                var thayThe = dsUngVien.Where(x => x != gvId).ToList();
                                if (thayThe.Count > 0)
                                {
                                    gen2.MaGiangVien = thayThe[_random.Next(thayThe.Count)];
                                }
                            }
                        }
                    }
                }
            }
        }

        return caThe;
    }
}
