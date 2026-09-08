using XepLichGiangDay.Core.Models;

namespace XepLichGiangDay.Core.Interfaces
{
    public interface IThuatToanXepLich
    {
        string TenThuatToan { get; }
        KetQuaXepLich GiaiQuyet(DauVaoXepLich dauVao);
    }
}