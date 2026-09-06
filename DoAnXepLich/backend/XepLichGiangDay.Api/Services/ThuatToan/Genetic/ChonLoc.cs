namespace XepLichGiangDay.Api.Services.ThuatToan.Genetic;

public class ChonLoc
{
    private readonly Random _random = new();

    /// <summary>
    /// Chọn lọc giải đấu (Tournament Selection): Chọn k cá thể ngẫu nhiên và lấy cá thể có điểm phạt thấp nhất.
    /// </summary>
    public CaThe ChonCaTheChaMe(List<CaThe> quanThe, int kichThuocGiaiDau = 3)
    {
        if (quanThe.Count == 0)
        {
            throw new InvalidOperationException("Quần thể rỗng.");
        }

        CaThe best = quanThe[_random.Next(quanThe.Count)];

        for (int i = 1; i < kichThuocGiaiDau; i++)
        {
            var doiThu = quanThe[_random.Next(quanThe.Count)];
            if (doiThu.DiemPhat < best.DiemPhat)
            {
                best = doiThu;
            }
        }

        return best;
    }
}
