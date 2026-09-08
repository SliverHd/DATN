import { useState } from 'react';
import { BangDieuKhienXepLich } from '../../components/BangDieuKhienXepLich';
import { MaTranThoiKhoaBieu } from '../../components/MaTranThoiKhoaBieu';
import { dichVuXepLich } from '../../api/dichVuXepLich'; // nếu bạn để ở src/services thì đổi thành '../../services/dichVuXepLich'
import type {
  LopHocPhanTKB,
  ChiTietPhanCong,
  KetQuaDanhGiaRangBuocMem
} from '../../types/kieuDuLieuXepLich';

// Dữ liệu mẫu lớp học phần ban đầu để vẽ lưới ma trận
const DANH_SACH_LOP_BAN_DAU: LopHocPhanTKB[] = [
  { maLopHocPhan: 'LHP01', tenLopHocPhan: 'Cơ học kết cấu 1', maMonHoc: 'XD01', maPhong: 'H1-201', thuTrongTuan: 2, tietBatDau: 1, soTiet: 3 },
  { maLopHocPhan: 'LHP02', tenLopHocPhan: 'Bê tông cốt thép 1', maMonHoc: 'XD02', maPhong: 'H1-202', thuTrongTuan: 2, tietBatDau: 4, soTiet: 3 },
  { maLopHocPhan: 'LHP03', tenLopHocPhan: 'Kiến trúc dân dụng', maMonHoc: 'XD03', maPhong: 'H2-101', thuTrongTuan: 3, tietBatDau: 1, soTiet: 3, maGVPhanCongTruoc: 'GV01', tenGVPhanCongTruoc: 'Thầy Thoan' },
  { maLopHocPhan: 'LHP04', tenLopHocPhan: 'Sức bền vật liệu', maMonHoc: 'XD04', maPhong: 'H1-301', thuTrongTuan: 4, tietBatDau: 7, soTiet: 3 },
  { maLopHocPhan: 'LHP05', tenLopHocPhan: 'Tin học chuyên ngành XD', maMonHoc: 'XD05', maPhong: 'PM-102', thuTrongTuan: 6, tietBatDau: 1, soTiet: 3 }
];

export default function ChayXepLich() {
  const [danhSachLop] = useState<LopHocPhanTKB[]>(DANH_SACH_LOP_BAN_DAU);
  const [danhSachPhanCong, setDanhSachPhanCong] = useState<ChiTietPhanCong[]>([]);
  const [dangXuLy, setDangXuLy] = useState(false);
  const [thoiGianChayMs, setThoiGianChayMs] = useState<number | undefined>(undefined);
  const [diemDanhGiaMem, setDiemDanhGiaMem] = useState<KetQuaDanhGiaRangBuocMem | null>(null);
  const [hocKyHienTai, setHocKyHienTai] = useState('HK1-2026-2027');

  // Kích hoạt chạy thuật toán
  const handleChayXepLich = async (maHocKi: string) => {
    setDangXuLy(true);
    setHocKyHienTai(maHocKi);
    try {
      const phanHoi = await dichVuXepLich.chayTheoHocKy(maHocKi);
      setDanhSachPhanCong(phanHoi.ketQua.danhSachPhanCong);
      setThoiGianChayMs(phanHoi.ketQua.thoiGianChayMs);
      setDiemDanhGiaMem(phanHoi.danhGiaRangBuocMem);
    } catch (error) {
      console.error('Lỗi khi gọi API xếp lịch:', error);
      alert('Không thể kết nối đến Backend! Hãy kiểm tra backend đang chạy ở cổng 5005.');
    } finally {
      setDangXuLy(false);
    }
  };

  // Đổi giảng viên và khóa bán tự động
  const handleKhoaGiangVien = async (maLop: string, maGV: string, tenGV: string) => {
    try {
      await dichVuXepLich.khoaGiangVien(hocKyHienTai, maLop, maGV, tenGV);
      setDanhSachPhanCong(prev =>
        prev.map(item =>
          item.maLopHocPhan === maLop
            ? { ...item, maGV, tenGV, daKhoaThuCong: true }
            : item
        )
      );
      alert(`Đã khóa thành công lớp ${maLop} cho ${tenGV}!`);
    } catch (error) {
      console.error('Lỗi khóa giảng viên:', error);
      alert('Lỗi khi lưu trạng thái khóa!');
    }
  };

  return (
    <div style={{ padding: '20px' }}>
      <h2 style={{ color: '#1890ff', marginBottom: '8px' }}>
        XẾP THỜI KHÓA BIỂU BỘ MÔN (GOOGLE CP-SAT)
      </h2>
      <p style={{ color: '#666', marginBottom: '20px' }}>
        Phân công giảng viên tự động, đo lường thời gian thực tế và can thiệp bán tự động.
      </p>

      {/* Bảng điều khiển */}
      <BangDieuKhienXepLich
        onChayXepLich={handleChayXepLich}
        dangXuLy={dangXuLy}
        thoiGianChayMs={thoiGianChayMs}
      />

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
            marginBottom: '20px'
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
            <b>⚖️ Phạt lệch tải:</b>{' '}
            <span style={{ color: '#ff4d4f' }}>-{diemDanhGiaMem.diemPhatLechDinhMuc}</span>
          </div>
          <div>
            <b>📅 Phạt rải lịch:</b>{' '}
            <span style={{ color: '#fa8c16' }}>-{diemDanhGiaMem.diemPhatRaiLich}</span>
          </div>
        </div>
      )}

      {/* Lưới Ma trận TKB */}
      <MaTranThoiKhoaBieu
        danhSachLop={danhSachLop}
        danhSachPhanCong={danhSachPhanCong}
        onKhoaGiangVien={handleKhoaGiangVien}
      />
    </div>
  );
}