using System.Diagnostics;
using XepLichGiangDay.Core.Interfaces;
using XepLichGiangDay.Core.Models;

namespace XepLichGiangDay.Engine.CPSat
{
    public class BoGiaiCPSat : IThuatToanXepLich
    {
        public string TenThuatToan => "Google CP-SAT Solver";

        public KetQuaXepLich GiaiQuyet(DauVaoXepLich dauVao)
        {
            var dongHo = Stopwatch.StartNew();
            var ketQua = new KetQuaXepLich
            {
                TenThuatToan = this.TenThuatToan,
                KhaThi = true,
                DiemChatLuong = 0,
                SoViPhamCung = 0,
                DanhSachPhanCong = new System.Collections.Generic.List<ChiTietPhanCong>()
            };

            // =========================================================================
            // CHUẨN BỊ NHẬN DỮ LIỆU ĐẦU VÀO ĐƯỢC IMPORT TỪ CSDL / FILE EXCEL:
            // - dauVao.DanhSachLop: Danh sách các Lớp học phần đã import
            // - dauVao.DanhSachGiangVien: Danh sách Giảng viên, Năng lực & Lịch bận
            // =========================================================================
            // TODO: Phát triển mô hình bài toán Google OR-Tools CP-SAT tại đây.
            // =========================================================================

            dongHo.Stop();
            ketQua.ThoiGianChayMs = dongHo.ElapsedMilliseconds;
            return ketQua;
        }
    }
}