using System.Collections.Generic;

namespace XepLichGiangDay.Core.Models
{
    // 1. Dữ liệu Lớp học phần cố định từ trường (do SV1 import)
    public class LopHocPhanTKB
    {
        public string MaLopHocPhan { get; set; } = string.Empty;
        public string TenLopHocPhan { get; set; } = string.Empty;
        public string MaMonHoc { get; set; } = string.Empty;
        public string TenMonHoc { get; set; } = string.Empty;
        public string MaPhong { get; set; } = string.Empty;
        public int ThuTrongTuan { get; set; } // 2 -> 7
        public int TietBatDau { get; set; }   // 1, 4, 7, 10
        public int SoTiet { get; set; } = 3;
        public string CaHoc { get; set; } = string.Empty;

        // Bán tự động (Pre-assignment): Gán trước/Khóa nếu Trưởng bộ môn chỉ định
        public string? MaGVPhanCongTruoc { get; set; }
        public string? TenGVPhanCongTruoc { get; set; }
    }

    // 2. Lịch bận cố định (Ràng buộc cứng)
    public class LichBanItem
    {
        public int ThuTrongTuan { get; set; }
        public int Tiet { get; set; }
        public string LyDo { get; set; } = string.Empty;
    }

    // 3. Nguyện vọng giảng dạy (Ràng buộc mềm: +10 Thích, -10 Tránh)
    public class NguyenVongItem
    {
        public int ThuTrongTuan { get; set; }
        public int Tiet { get; set; }
        public int DiemUuTien { get; set; }
    }

    // 4. Thông tin giảng viên (SV2 quản lý)
    public class GiangVienModel
    {
        public string MaGV { get; set; } = string.Empty;
        public string TenGV { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int SoTietToiDaTuan { get; set; } = 20;
        public int SoTietToiThieuTuan { get; set; } = 6;
        public List<string> DanhSachMaMonHoc { get; set; } = new();
        public List<LichBanItem> DanhSachLichBan { get; set; } = new();
        public List<NguyenVongItem> DanhSachNguyenVong { get; set; } = new();
    }

    // 5. Gói dữ liệu đầu vào chung cho thuật toán
    public class DauVaoXepLich
    {
        public string MaHocKi { get; set; } = string.Empty;
        public List<LopHocPhanTKB> DanhSachLop { get; set; } = new();
        public List<GiangVienModel> DanhSachGiangVien { get; set; } = new();
    }

    // 6. Kết quả phân công cho từng lớp
    public class ChiTietPhanCong
    {
        public string MaLopHocPhan { get; set; } = string.Empty;
        public string MaGV { get; set; } = string.Empty;
        public string TenGV { get; set; } = string.Empty;
        public bool DaKhoaThuCong { get; set; }
    }

    // 7. Gói kết quả đầu ra chung
    public class KetQuaXepLich
    {
        public string TenThuatToan { get; set; } = "Google CP-SAT Solver";
        public bool KhaThi { get; set; }
        public double ThoiGianChayMs { get; set; }
        public double DiemChatLuong { get; set; }
        public int SoViPhamCung { get; set; }
        public List<ChiTietPhanCong> DanhSachPhanCong { get; set; } = new();
    }

    // 8. DTO tiếp nhận yêu cầu gán/đổi thủ công từ Trưởng bộ môn
    public class YeuCauDoiGiangVienThuCong
    {
        public string MaHocKi { get; set; } = string.Empty;
        public string MaLopHocPhan { get; set; } = string.Empty;
        public string MaGVMoi { get; set; } = string.Empty;
        public string TenGVMoi { get; set; } = string.Empty;
        public bool DaKhoa { get; set; } = true;
    }
}