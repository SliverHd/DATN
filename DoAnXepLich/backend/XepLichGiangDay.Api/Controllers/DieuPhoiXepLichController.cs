using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XepLichGiangDay.Api.Data;
using XepLichGiangDay.Api.Models.Entities;
using XepLichGiangDay.Core.Evaluation;
using XepLichGiangDay.Core.Models;
using XepLichGiangDay.Engine.CPSat;

namespace XepLichGiangDay.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DieuPhoiXepLichController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DieuPhoiXepLichController(AppDbContext context)
        {
            _context = context;
        }

        // Kích hoạt giải thuật theo học kỳ (truyền ID số hoặc Mã chuỗi học kỳ)
        [HttpPost("chay-theo-hoc-ky")]
        public async Task<IActionResult> ChayTheoHocKy([FromQuery] string maHocKi)
        {
            if (string.IsNullOrWhiteSpace(maHocKi))
            {
                return BadRequest("Vui lòng chọn học kỳ để xếp lịch.");
            }

            int maHocKyInt = 0;
            int.TryParse(maHocKi, out maHocKyInt);

            // Tìm học kỳ tương ứng trong CSDL
            var hocKyObj = await _context.HocKy.FirstOrDefaultAsync(x => 
                (maHocKyInt > 0 && x.MaHocKy == maHocKyInt) || 
                x.TenHocKy.Equals(maHocKi, StringComparison.OrdinalIgnoreCase));

            int idHocKyThucTe = hocKyObj?.MaHocKy ?? (maHocKyInt > 0 ? maHocKyInt : 0);

            if (idHocKyThucTe <= 0)
            {
                return BadRequest("Học kỳ không tồn tại trong hệ thống.");
            }

            // 1. Tải danh sách Lớp học phần thực tế từ CSDL đã import
            var dsLopDb = await (from lop in _context.LopHocPhan
                                 where lop.MaHocKy == idHocKyThucTe
                                 join hp in _context.HocPhan on lop.MaHocPhan equals hp.MaHocPhan into hpGroup
                                 from hp in hpGroup.DefaultIfEmpty()
                                 join ct in _context.ChiTietThoiKhoaBieu on lop.MaLopHocPhan equals ct.MaLopHocPhan into ctGroup
                                 from ct in ctGroup.DefaultIfEmpty()
                                 select new
                                 {
                                     lop.MaLopHocPhan,
                                     lop.MaLopHocPhanTruong,
                                     lop.TenLop,
                                     lop.MaHocPhan,
                                     TenHocPhan = hp != null ? hp.TenHocPhan : "",
                                     Thu = ct != null && ct.Thu > 0 ? ct.Thu : 2,
                                     TietBatDau = ct != null && ct.TietBatDau > 0 ? ct.TietBatDau : 1,
                                     SoTiet = ct != null && ct.TietKetThuc >= ct.TietBatDau ? (ct.TietKetThuc - ct.TietBatDau + 1) : 3,
                                     PhongHoc = ct != null ? ct.PhongHoc : ""
                                 }).ToListAsync();

            // 2. Tải danh sách Giảng viên thực tế từ CSDL
            var dsGiangVienDb = await _context.GiangVien
                .Where(x => x.TrangThai == "DangLamViec" || x.TrangThai == "DangHoatDong")
                .ToListAsync();

            var dsNangLuc = await _context.NangLucGiangDay.ToListAsync();
            var dsLichBan = await _context.LichBanGiangVien.Where(x => x.MaHocKy == idHocKyThucTe).ToListAsync();
            var dsNguyenVong = await _context.NguyenVongGiangVien.Where(x => x.MaHocKy == idHocKyThucTe).ToListAsync();
            var dsDinhMuc = await _context.DinhMucGiangVien.Where(x => x.MaHocKy == idHocKyThucTe).ToListAsync();

            // Đã phân công trước / Khóa thủ công
            var dsPhanCongSan = await _context.PhanCongGiangDay
                .Where(x => x.MaHocKy == idHocKyThucTe)
                .ToListAsync();

            // 3. Đóng gói dữ liệu đầu vào thực tế
            var dauVao = new DauVaoXepLich
            {
                MaHocKi = maHocKi
            };

            foreach (var item in dsLopDb)
            {
                var pcSan = dsPhanCongSan.FirstOrDefault(x => x.MaLopHocPhan == item.MaLopHocPhan);
                string? maGvSan = pcSan != null ? pcSan.MaGiangVien.ToString() : null;

                dauVao.DanhSachLop.Add(new LopHocPhanTKB
                {
                    MaLopHocPhan = item.MaLopHocPhanTruong ?? $"LHP_{item.MaLopHocPhan}",
                    TenLopHocPhan = string.IsNullOrWhiteSpace(item.TenHocPhan) ? item.TenLop : item.TenHocPhan,
                    MaMonHoc = item.MaHocPhan.ToString(),
                    MaPhong = string.IsNullOrWhiteSpace(item.PhongHoc) ? "Chưa gán" : item.PhongHoc,
                    ThuTrongTuan = item.Thu,
                    TietBatDau = item.TietBatDau,
                    SoTiet = item.SoTiet,
                    MaGVPhanCongTruoc = maGvSan
                });
            }

            foreach (var gv in dsGiangVienDb)
            {
                var monDienClass = dsNangLuc
                    .Where(nl => nl.MaGiangVien == gv.MaGiangVien)
                    .Select(nl => nl.MaHocPhan.ToString())
                    .ToList();

                var dm = dsDinhMuc.FirstOrDefault(x => x.MaGiangVien == gv.MaGiangVien);

                var gvModel = new GiangVienModel
                {
                    MaGV = gv.MaGiangVien.ToString(),
                    TenGV = gv.HoTen,
                    Email = gv.Email,
                    SoTietToiDaTuan = dm?.SoTietToiDa ?? 20,
                    SoTietToiThieuTuan = dm?.SoTietToiThieu ?? 6,
                    DanhSachMaMonHoc = monDienClass
                };

                // Lịch bận
                var lbList = dsLichBan.Where(x => x.MaGiangVien == gv.MaGiangVien).ToList();
                foreach (var lb in lbList)
                {
                    for (int t = lb.TietBatDau; t <= (lb.TietKetThuc > 0 ? lb.TietKetThuc : lb.TietBatDau); t++)
                    {
                        gvModel.DanhSachLichBan.Add(new LichBanItem
                        {
                            ThuTrongTuan = lb.Thu,
                            Tiet = t,
                            LyDo = lb.LyDo ?? "Ban"
                        });
                    }
                }

                // Nguyện vọng
                var nvList = dsNguyenVong.Where(x => x.MaGiangVien == gv.MaGiangVien).ToList();
                foreach (var nv in nvList)
                {
                    if (nv.Thu.HasValue && nv.TietBatDau.HasValue)
                    {
                        gvModel.DanhSachNguyenVong.Add(new NguyenVongItem
                        {
                            ThuTrongTuan = nv.Thu.Value,
                            Tiet = nv.TietBatDau.Value,
                            DiemUuTien = nv.MucDo > 0 ? nv.MucDo * 2 : 10
                        });
                    }
                }

                dauVao.DanhSachGiangVien.Add(gvModel);
            }

            // 4. Chạy bộ giải CP-SAT
            var boGiai = new BoGiaiCPSat();
            var ketQua = boGiai.GiaiQuyet(dauVao);

            // 5. Đánh giá chất lượng phân công
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

        // Bán tự động: Gán thủ công và khóa lớp giảng viên
        [HttpPost("khoa-giang-vien-thu-cong")]
        public async Task<IActionResult> KhoaGiangVienThuCong([FromBody] YeuCauDoiGiangVienThuCong yeuCau)
        {
            if (string.IsNullOrEmpty(yeuCau.MaLopHocPhan) || string.IsNullOrEmpty(yeuCau.MaGVMoi))
            {
                return BadRequest("Thông tin phân công thủ công không hợp lệ!");
            }

            // Lưu trạng thái phân công thủ công vào CSDL
            if (int.TryParse(yeuCau.MaLopHocPhan, out int maLhpInt) && int.TryParse(yeuCau.MaGVMoi, out int maGvInt))
            {
                var pc = await _context.PhanCongGiangDay.FirstOrDefaultAsync(x => x.MaLopHocPhan == maLhpInt);
                if (pc == null)
                {
                    _context.PhanCongGiangDay.Add(new PhanCongGiangDay
                    {
                        MaLopHocPhan = maLhpInt,
                        MaGiangVien = maGvInt,
                        MaHocKy = int.TryParse(yeuCau.MaHocKi, out int hk) ? hk : 1,
                        TrangThai = yeuCau.DaKhoa ? "DaKhoa" : "TamGan"
                    });
                }
                else
                {
                    pc.MaGiangVien = maGvInt;
                    pc.TrangThai = yeuCau.DaKhoa ? "DaKhoa" : "TamGan";
                }
                await _context.SaveChangesAsync();
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