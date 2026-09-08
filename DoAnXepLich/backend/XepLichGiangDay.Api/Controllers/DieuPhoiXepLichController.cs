using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using XepLichGiangDay.Core.Evaluation;
using XepLichGiangDay.Core.Models;
using XepLichGiangDay.Engine.CPSat;

namespace XepLichGiangDay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DieuPhoiXepLichController : ControllerBase
    {
        // Kích hoạt giải thuật theo mã học kỳ
        [HttpPost("chay-theo-hoc-ky")]
        public IActionResult ChayTheoHocKy([FromQuery] string maHocKi = "HK1-2026-2027")
        {
            // Dữ liệu mẫu ban đầu (khi chưa kết nối bảng Excel của SV1)
            var dauVao = new DauVaoXepLich
            {
                MaHocKi = maHocKi,
                DanhSachLop = new List<LopHocPhanTKB>
                {
                    new() { MaLopHocPhan = "LHP01", TenLopHocPhan = "Cơ học kết cấu 1", MaMonHoc = "XD01", ThuTrongTuan = 2, TietBatDau = 1, SoTiet = 3, MaPhong = "H1-201" },
                    new() { MaLopHocPhan = "LHP02", TenLopHocPhan = "Bê tông cốt thép 1", MaMonHoc = "XD02", ThuTrongTuan = 2, TietBatDau = 4, SoTiet = 3, MaPhong = "H1-202" },
                    new() { MaLopHocPhan = "LHP03", TenLopHocPhan = "Kiến trúc dân dụng", MaMonHoc = "XD03", ThuTrongTuan = 3, TietBatDau = 1, SoTiet = 3, MaPhong = "H2-101", MaGVPhanCongTruoc = "GV01", TenGVPhanCongTruoc = "Thầy Thoan" },
                    new() { MaLopHocPhan = "LHP04", TenLopHocPhan = "Sức bền vật liệu", MaMonHoc = "XD04", ThuTrongTuan = 4, TietBatDau = 7, SoTiet = 3, MaPhong = "H1-301" },
                    new() { MaLopHocPhan = "LHP05", TenLopHocPhan = "Tin học chuyên ngành XD", MaMonHoc = "XD05", ThuTrongTuan = 6, TietBatDau = 1, SoTiet = 3, MaPhong = "PM-102" }
                },
                DanhSachGiangVien = new List<GiangVienModel>
                {
                    new() { MaGV = "GV01", TenGV = "Thầy Thoan", SoTietToiDaTuan = 18, DanhSachMaMonHoc = new() { "XD01", "XD03" } },
                    new() { MaGV = "GV02", TenGV = "Thầy Cường", SoTietToiDaTuan = 20, DanhSachMaMonHoc = new() { "XD02", "XD04" } },
                    new() { MaGV = "GV03", TenGV = "Cô Mai Hồng", SoTietToiDaTuan = 15, DanhSachMaMonHoc = new() { "XD01", "XD05" } }
                }
            };

            var boGiai = new BoGiaiCPSat();
            var ketQua = boGiai.GiaiQuyet(dauVao);

            var boChamDiem = new ChamDiemRangBuocMem();
            var danhGiaMem = boChamDiem.DanhGia(dauVao, ketQua.DanhSachPhanCong);

            return Ok(new
            {
                MaHocKi = dauVao.MaHocKi,
                ThuatToanSuDung = boGiai.TenThuatToan,
                KetQua = ketQua,
                DanhGiaRangBuocMem = danhGiaMem
            });
        }

        // Bán tự động: Gán thủ công và khóa lớp
        [HttpPost("khoa-giang-vien-thu-cong")]
        public IActionResult KhoaGiangVienThuCong([FromBody] YeuCauDoiGiangVienThuCong yeuCau)
        {
            if (string.IsNullOrEmpty(yeuCau.MaLopHocPhan) || string.IsNullOrEmpty(yeuCau.MaGVMoi))
            {
                return BadRequest("Thông tin phân công thủ công không hợp lệ!");
            }

            return Ok(new
            {
                ThongBao = $"Đã khóa lớp {yeuCau.MaLopHocPhan} cho giảng viên {yeuCau.TenGVMoi} ({yeuCau.MaGVMoi}).",
                yeuCau.MaHocKi,
                yeuCau.MaLopHocPhan,
                yeuCau.MaGVMoi,
                yeuCau.TenGVMoi,
                DaKhoa = yeuCau.DaKhoa
            });
        }
    }
}