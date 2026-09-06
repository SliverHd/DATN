namespace XepLichGiangDay.Api.Services.RangBuoc;

public class BoDanhGiaPhuongAn
{
    public double TinhDiemNguyenVong(int maGiangVien, int maLopHocPhan)// Ưu tiên nguyện vọng và phân cấp ưu tiên giảng viên cơ hữu / thỉnh giảng (Mục 2.2 & 2.7).
    {
        _ = (maGiangVien, maLopHocPhan);
        return 0;
    }

    public double TinhDiemGomLich(int maGiangVien) //Tối ưu số ngày đến trường và giảm tiết trống giữa ca (Mục 2.1 & 2.2).
    {
        _ = maGiangVien;
        return 0;
    }

    public double TinhDiemCanBangTai(int maGiangVien)//Cân bằng định mức giờ dạy giữa các thầy cô trong bộ môn (Mục 2.5).
    {
        _ = maGiangVien;
        return 0;
    }

    public double TinhTongDiemPhat(IEnumerable<double> danhSachDiem)
    {
        return danhSachDiem.Sum();
    }
}
