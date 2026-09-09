import { useState, useEffect } from 'react';
import { BangDieuKhienXepLich } from '../../components/BangDieuKhienXepLich';
import { MaTranThoiKhoaBieu } from '../../components/MaTranThoiKhoaBieu';
import { dichVuXepLich } from '../../api/dichVuXepLich';
import { hocKyApi } from '../../api/hocKyApi';
import { thoiKhoaBieuApi } from '../../api/thoiKhoaBieuApi';
import type {
  LopHocPhanTKB,
  ChiTietPhanCong,
  KetQuaDanhGiaRangBuocMem
} from '../../types/kieuDuLieuXepLich';
import type { HocKy } from '../../types/hocKy';

export default function ChayXepLich() {
  const [danhSachLop, setDanhSachLop] = useState<LopHocPhanTKB[]>([]);
  const [danhSachPhanCong, setDanhSachPhanCong] = useState<ChiTietPhanCong[]>([]);
  const [dangXuLy, setDangXuLy] = useState(false);
  const [thoiGianChayMs, setThoiGianChayMs] = useState<number | undefined>(undefined);
  const [diemDanhGiaMem, setDiemDanhGiaMem] = useState<KetQuaDanhGiaRangBuocMem | null>(null);
  const [hocKyHienTai, setHocKyHienTai] = useState('HK1-2026-2027');
  const [danhSachHocKy, setDanhSachHocKy] = useState<HocKy[]>([]);

  // Tải danh sách Học kỳ từ CSDL
  useEffect(() => {
    const taiHocKy = async () => {
      try {
        const res = await hocKyApi.getAll();
        if (res.data && res.data.length > 0) {
          setDanhSachHocKy(res.data);
          setHocKyHienTai(res.data[0].tenHocKy || 'HK1-2026-2027');
          await taiLopHocPhanHocKy(res.data[0].tenHocKy, res.data);
        }
      } catch {
        console.log('Chưa tải được danh sách học kỳ.');
      }
    };
    void taiHocKy();
  }, []);

  // Tải lớp học phần thực tế khi đổi học kỳ
  const taiLopHocPhanHocKy = async (maHkStr: string, listHk = danhSachHocKy) => {
    try {
      const hkObj = listHk.find((h) => h.tenHocKy === maHkStr);
      const res = await thoiKhoaBieuApi.getAll(hkObj?.maHocKy);
      if (res.data && res.data.length > 0) {
        const converted: LopHocPhanTKB[] = res.data.map((item) => ({
          maLopHocPhan: item.maLopHocPhanTruong || `LHP_${item.maLopHocPhan}`,
          tenLopHocPhan: item.tenHocPhan || item.tenLop,
          maMonHoc: String(item.maHocKy),
          maPhong: item.phongHoc || 'Chưa gán',
          thuTrongTuan: item.thu && item.thu > 0 ? item.thu : 2,
          tietBatDau: item.tietBatDau && item.tietBatDau > 0 ? item.tietBatDau : 1,
          soTiet: item.tietKetThuc >= item.tietBatDau ? (item.tietKetThuc - item.tietBatDau + 1) : 3,
        }));
        setDanhSachLop(converted);
      } else {
        setDanhSachLop([]);
      }
    } catch {
      setDanhSachLop([]);
    }
  };

  // Kích hoạt chạy thuật toán
  const handleChayXepLich = async (maHocKi: string) => {
    setDangXuLy(true);
    setHocKyHienTai(maHocKi);
    await taiLopHocPhanHocKy(maHocKi);

    try {
      const phanHoi = await dichVuXepLich.chayTheoHocKy(maHocKi);
      setDanhSachPhanCong(phanHoi.ketQua.danhSachPhanCong);
      setThoiGianChayMs(phanHoi.ketQua.thoiGianChayMs);
      setDiemDanhGiaMem(phanHoi.danhGiaRangBuocMem);
    } catch (error) {
      console.error('Lỗi khi gọi API xếp lịch:', error);
      alert('Không thể kết nối đến máy chủ Backend hoặc chưa có dữ liệu lớp học phần cho học kỳ này!');
    } finally {
      setDangXuLy(false);
    }
  };

  // Đổi giảng viên và khóa bán tự động
  const handleKhoaGiangVien = async (maLop: string, maGV: string, tenGV: string) => {
    try {
      await dichVuXepLich.khoaGiangVien(hocKyHienTai, maLop, maGV, tenGV);
      setDanhSachPhanCong((prev) =>
        prev.map((item) =>
          item.maLopHocPhan === maLop
            ? { ...item, maGV, tenGV, daKhoaThuCong: true }
            : item
        )
      );
      alert(`Đã khóa thành công lớp ${maLop} cho giảng viên ${tenGV}!`);
    } catch (error) {
      console.error('Lỗi khóa giảng viên:', error);
      alert('Lỗi khi lưu trạng thái khóa giảng viên!');
    }
  };

  return (
    <div style={{ padding: '20px' }}>
      <h2 style={{ color: '#1890ff', marginBottom: '8px' }}>
        XẾP THỜI KHÓA BIỂU BỘ MÔN (GOOGLE CP-SAT SOLVER)
      </h2>
      <p style={{ color: '#666', marginBottom: '20px' }}>
        Phân công giảng viên tự động, tối ưu hóa nguyện vọng, đo lường thời gian thực tế và can thiệp bán tự động.
      </p>

      {/* Bảng điều khiển */}
      <BangDieuKhienXepLich
        onChayXepLich={handleChayXepLich}
        dangXuLy={dangXuLy}
        thoiGianChayMs={thoiGianChayMs}
      />

      {/* Thông báo nếu chưa import dữ liệu */}
      {danhSachLop.length === 0 && (
        <div className="alert alert-info my-3">
          ℹ️ Chưa có dữ liệu lớp học phần cho học kỳ này. Vui lòng vào mục <strong>Thời khóa biểu (Import)</strong> để tải lên file Excel dữ liệu lớp học phần trước khi xếp lịch.
        </div>
      )}

      {/* Điểm chất lượng mềm */}
      {diemDanhGiaMem && (
        <div
          style={{
            display: 'flex',
            gap: '20px',
            backgroundColor: '#e6f7ff',
            border: '1px solid #91d5ff',
            borderRadius: '6px',
            padding: '12px 16px',
            marginBottom: '20px',
            flexWrap: 'wrap'
          }}
        >
          <div>
            <b>🏆 Điểm chất lượng:</b>{' '}
            <span style={{ color: '#1890ff', fontSize: '16px', fontWeight: 'bold' }}>
              {diemDanhGiaMem.tongDiemChatLuong}
            </span>
          </div>
          <div>
            <b>⭐ Hài lòng nguyện vọng:</b>{' '}
            <span style={{ color: '#52c41a' }}>+{diemDanhGiaMem.diemThoaManNguyenVong}</span>
          </div>
          <div>
            <b>⚖️ Phạt lệch định mức:</b>{' '}
            <span style={{ color: '#ff4d4f' }}>-{diemDanhGiaMem.diemPhatLechDinhMuc}</span>
          </div>
          <div>
            <b>📅 Phạt rải lịch:</b>{' '}
            <span style={{ color: '#faad14' }}>-{diemDanhGiaMem.diemPhatRaiLich}</span>
          </div>
        </div>
      )}

      {/* Ma trận TKB */}
      {danhSachLop.length > 0 && (
        <MaTranThoiKhoaBieu
          danhSachLop={danhSachLop}
          danhSachPhanCong={danhSachPhanCong}
          onKhoaGiangVien={handleKhoaGiangVien}
        />
      )}
    </div>
  );
}