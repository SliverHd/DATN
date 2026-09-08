using System;
using System.Collections.Generic;
using System.Linq;
using XepLichGiangDay.Core.Models;

namespace XepLichGiangDay.Core.Evaluation
{
    public class KetQuaDanhGiaRangBuocMem
    {
        public double TongDiemChatLuong { get; set; }
        public double DiemThoaManNguyenVong { get; set; }
        public double DiemPhatLechDinhMuc { get; set; }
        public double DiemPhatRaiLich { get; set; }
    }

    public class ChamDiemRangBuocMem
    {
        public KetQuaDanhGiaRangBuocMem DanhGia(DauVaoXepLich dauVao, List<ChiTietPhanCong> danhSachPhanCong)
        {
            var ketQua = new KetQuaDanhGiaRangBuocMem();
            var banDoLop = dauVao.DanhSachLop.ToDictionary(c => c.MaLopHocPhan);

            var lopTheoGV = danhSachPhanCong
                .GroupBy(a => a.MaGV)
                .ToDictionary(
                    g => g.Key,
                    g => g.Where(a => banDoLop.ContainsKey(a.MaLopHocPhan))
                          .Select(a => banDoLop[a.MaLopHocPhan])
                          .ToList()
                );

            foreach (var gv in dauVao.DanhSachGiangVien)
            {
                var danhSachLopCuaGV = lopTheoGV.GetValueOrDefault(gv.MaGV, new List<LopHocPhanTKB>());
                int tongSoTiet = danhSachLopCuaGV.Sum(c => c.SoTiet);

                // 1. Phạt lệch định mức tải tiết dạy
                int tietMucTieu = (gv.SoTietToiThieuTuan + gv.SoTietToiDaTuan) / 2;
                if (tongSoTiet < gv.SoTietToiThieuTuan)
                    ketQua.DiemPhatLechDinhMuc += (gv.SoTietToiThieuTuan - tongSoTiet) * 15;
                else if (tongSoTiet > gv.SoTietToiDaTuan)
                    ketQua.DiemPhatLechDinhMuc += (tongSoTiet - gv.SoTietToiDaTuan) * 20;
                else
                    ketQua.DiemPhatLechDinhMuc += Math.Abs(tongSoTiet - tietMucTieu) * 3;

                // 2. Thỏa mãn nguyện vọng cá nhân
                foreach (var c in danhSachLopCuaGV)
                {
                    var nv = gv.DanhSachNguyenVong.FirstOrDefault(p =>
                        p.ThuTrongTuan == c.ThuTrongTuan && p.Tiet == c.TietBatDau);
                    if (nv != null)
                        ketQua.DiemThoaManNguyenVong += nv.DiemUuTien;
                }

                // 3. Phạt phân tán lịch dạy trong tuần (không gom lịch)
                int soNgayDiDay = danhSachLopCuaGV.Select(c => c.ThuTrongTuan).Distinct().Count();
                if (soNgayDiDay >= 3 && tongSoTiet <= 6)
                    ketQua.DiemPhatRaiLich += 25;
            }

            ketQua.TongDiemChatLuong = ketQua.DiemThoaManNguyenVong - (ketQua.DiemPhatLechDinhMuc + ketQua.DiemPhatRaiLich);
            return ketQua;
        }
    }
}