namespace XepLichGiangDay.Api.Services.ThuatToan.Genetic;

public class LaiGhep
{
    private readonly Random _random = new();

    /// <summary>
    /// Lai ghép đồng nhất (Uniform Crossover): Mỗi gen của con có 50% cơ hội thừa hưởng từ cha hoặc mẹ.
    /// </summary>
    public CaThe TaoCon(CaThe cha, CaThe me, double tyLeLaiGhep)
    {
        var con = new CaThe();
        int soLuongGen = cha.DanhSachGen.Count;

        if (_random.NextDouble() > tyLeLaiGhep)
        {
            // Không lai ghép: sao chép nguyên vẹn từ cha
            foreach (var gen in cha.DanhSachGen)
            {
                con.DanhSachGen.Add(new Gen
                {
                    MaLopHocPhan = gen.MaLopHocPhan,
                    MaGiangVien = gen.MaGiangVien
                });
            }
            return con;
        }

        // Thực hiện lai ghép đồng nhất
        for (int i = 0; i < soLuongGen; i++)
        {
            var genNguon = _random.NextDouble() < 0.5 ? cha.DanhSachGen[i] : me.DanhSachGen[i];
            con.DanhSachGen.Add(new Gen
            {
                MaLopHocPhan = genNguon.MaLopHocPhan,
                MaGiangVien = genNguon.MaGiangVien
            });
        }

        return con;
    }
}
